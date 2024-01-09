using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Utility.Kafka.Abstractions.Clients;

namespace Utility.Kafka.Clients;

public class ConsumerClient : IConsumerClient {

    readonly IConsumer<Null, string> _consumer;
    readonly ILogger<ConsumerClient> _logger;

    bool _disposedValue;

    public ConsumerClient(IOptions<KafkaConsumerClientOptions> options, ILogger<ConsumerClient> logger) {
        _logger = logger;
        _consumer = new ConsumerBuilder<Null, string>(GetConsumerConfig(options)).Build();
    }

    private ConsumerConfig GetConsumerConfig(IOptions<KafkaConsumerClientOptions> options) {
        ConsumerConfig consumerConfig = new ConsumerConfig();
        consumerConfig.BootstrapServers = options.Value.BootstrapServers;
        consumerConfig.GroupId = options.Value.GroupId;
        consumerConfig.ClientId = Dns.GetHostName();
        consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
        consumerConfig.EnableAutoCommit = false;
        consumerConfig.AutoCommitIntervalMs = 0;
        consumerConfig.AllowAutoCreateTopics = false;
        //consumerConfig.EnableAutoOffsetStore = false;

        _logger.LogInformation("Kafka ConsumerConfig: {consumerConfig}", JsonSerializer.Serialize(consumerConfig));

        return consumerConfig;
    }

    /// <inheritdoc/>
    public List<string> GetCurrentSubscription() => _consumer.Subscription;

    /// <inheritdoc/>
    public void Subscribe(IEnumerable<string> topics) {
        try {
            _logger.LogInformation("Sottoscrizione ai seguenti topic: '{topics}'...", string.Join("', '", topics));
            _consumer.Subscribe(topics);
        } catch (Exception ex) {
            _logger.LogError(ex, "Exception sollevata all'interno del metodo {methodName}. Exception Message: {message}", nameof(Subscribe), ex.Message);
            throw;
        }
        _logger.LogInformation("Sottoscrizione completata!");
    }

    /// <inheritdoc/>
    public void Subscribe(string topic) {
        Subscribe(new List<string>() { topic });
    }

    /// <inheritdoc/>
    public void Unsubscribe() {
        try {
            _logger.LogInformation("Annullamento alle sottoscrizioni correnti...");
            _consumer.Unsubscribe();
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Exception sollevata all'interno del metodo {methodName}. Exception Message: {message}", nameof(Unsubscribe), ex.Message);
            throw;
        }
        _logger.LogInformation("Annullamento alle sottoscrizioni correnti completata");
    }

    /// <inheritdoc/>
    public Task<ConsumeResult<Null, string>> ConsumeAsync(CancellationToken cancellationToken) {
        return Task.Run(() => {
            ConsumeResult<Null, string> result;
            try {
                _logger.LogInformation("Poll for new messages...");
                result = _consumer.Consume(cancellationToken);
            } catch (ConsumeException ex) {
                _logger.LogError(ex, "ConsumeException sollevata all'interno del metodo {methodName}: {reason}", nameof(ConsumeAsync), ex.Error.Reason);
                throw;
            } catch (KafkaException ex) {
                _logger.LogError(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", nameof(ConsumeAsync), ex.Error.Reason);
                throw;
            } catch (OperationCanceledException ex) {
                _logger.LogError(ex, "OperationCanceledException sollevata all'interno del metodo {methodName}: {message}", nameof(ConsumeAsync), ex.Message);
                throw;
            } catch (Exception ex) {
                _logger.LogError(ex, "Exception sollevata all'interno del metodo {methodName}: {message}", nameof(ConsumeAsync), ex.Message);
                throw;
            }
            _logger.LogInformation("Consumato il seguente messagio: {result}", JsonSerializer.Serialize(result));
            return result;
        });
    }

    /// <inheritdoc/>
    public async Task<bool> ConsumeInLoopAsync(string topic, Func<ConsumeResult<Null, string>, Task> comsumerOperationsAsync, CancellationToken cancellationToken = default) {
        return await ConsumeInLoopAsync(new List<string>() { topic }, comsumerOperationsAsync, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> ConsumeInLoopAsync(IEnumerable<string> topics, Func<ConsumeResult<Null, string>, Task> comsumerOperationsAsync, CancellationToken cancellationToken = default) {
        _logger.LogInformation("START Kafka ConsumeInLoopAsync");

        // Sottoscrizione alla lista di topic
        Subscribe(topics);

        ConsumeResult<Null, string>? result = null;

        // Consume Loop
        while (!cancellationToken.IsCancellationRequested) {

            try {

                // Lettura del messaggio
                result = await ConsumeAsync(cancellationToken);

                try {
                    // Elaborazione del messaggio
                    await comsumerOperationsAsync(result);
                } catch (Exception ex) {
                    _logger.LogWarning(ex, "Exception sollevata all'interno della Func {funcName}, per il seguente ConsumeResult: {result}. Exception Message: {message}",
                        nameof(comsumerOperationsAsync), JsonSerializer.Serialize(result), ex.Message);
                    throw;
                }

                _logger.LogInformation("Func {funcName} completata!", nameof(comsumerOperationsAsync));

            } catch (Exception ex) {
                _logger.LogError(ex, "Exception sollevata all'interno del metodo {methodName}. Exception Message: {message}",
                    nameof(ConsumeInLoopAsync), ex.Message);
            }

            Commit(result);

        }

        _logger.LogInformation("END Kafka ConsumeInLoopAsync");

        return true;
    }

    /// <inheritdoc/>
    public void Commit(ConsumeResult<Null, string>? result) {
        try {
            if (result != null) {
                _logger.LogDebug("Commit offset: {result}", JsonSerializer.Serialize(result));
                _consumer.Commit(result);
                //_consumer.StoreOffset(result);
            }
        } catch (TopicPartitionOffsetException ex) {
            _logger.LogCritical(ex, "TopicPartitionOffsetException sollevata all'interno del metodo {methodName}: {reason}", nameof(Commit), ex.Error.Reason);
            throw;
        } catch (KafkaException ex) {
            _logger.LogCritical(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", nameof(Commit), ex.Error.Reason);
            throw;
        } catch (Exception ex) {
            _logger.LogCritical(ex, "Exception sollevata all'interno del metodo {methodName}: {message}", nameof(Commit), ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // Eliminare lo stato gestito (oggetti gestiti)
                try {
                    _consumer?.Close(); // Close the consumer and leave the group safely.
                    _consumer?.Dispose();
                } catch (KafkaException ex) {
                    _logger.LogCritical(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", nameof(Dispose), ex.Error.Reason);
                    throw;
                } catch (Exception ex) {
                    _logger.LogCritical(ex, "Exception sollevata all'interno del metodo {methodName}: {message}", nameof(Dispose), ex.Message);
                    throw;
                }
            }
            // Liberare risorse non gestite (oggetti non gestiti) ed eseguire l'override del finalizzatore
            // Impostare campi di grandi dimensioni su Null
            _disposedValue = true;
        }
    }

    // // Eseguire l'override del finalizzatore solo se 'Dispose(bool disposing)' contiene codice per liberare risorse non gestite
    // ~ConsumerClient()
    // {
    //     // Non modificare questo codice. Inserire il codice di pulizia nel metodo 'Dispose(bool disposing)'
    //     Dispose(disposing: false);
    // }

    /// <inheritdoc/>
    public void Dispose() {
        // Non modificare questo codice. Inserire il codice di pulizia nel metodo 'Dispose(bool disposing)'
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}

