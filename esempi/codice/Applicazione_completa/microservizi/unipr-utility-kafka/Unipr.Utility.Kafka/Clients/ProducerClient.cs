using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Utility.Kafka.Abstractions.Clients;

namespace Utility.Kafka.Clients;

public class ProducerClient : IProducerClient {

    readonly IProducer<Null, string> _producer;
    readonly ILogger<ProducerClient> _logger;

    bool _disposedValue;

    public ProducerClient(IOptions<KafkaProducerClientOptions> options, ILogger<ProducerClient> logger) {
        _logger = logger;
        _producer = new ProducerBuilder<Null, string>(GetProducerConfig(options)).Build();
    }

    private ProducerConfig GetProducerConfig(IOptions<KafkaProducerClientOptions> options) {
        ProducerConfig producerConfig = new ProducerConfig();
        producerConfig.BootstrapServers = options.Value.BootstrapServers;
        producerConfig.ClientId = Dns.GetHostName();
        producerConfig.Acks = Acks.All;

        //consumerConfig.MessageSendMaxRetries // Tentativi di invio in caso di errore
        //consumerConfig.Partitioner = PartitionAssignmentStrategy.RoundRobin; // Default

        _logger.LogInformation("Kafka ProducerConfig: {producerConfig}", JsonSerializer.Serialize(producerConfig));

        return producerConfig;
    }

    /// <inheritdoc/>
    public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default) {
        await ProduceAsync(topic, message, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task ProduceAsync(string topic, int partition, string message, CancellationToken cancellationToken = default) {
        await ProduceAsync(topic, message, partition, cancellationToken);
    }

    private async Task ProduceAsync(string topic, string message, int? partition = null, CancellationToken cancellationToken = default) {
        // In caso di errore nell'invio del messaggio, si ripete l'invio in base alle regole specificate per l'_asyncPolicy 
        DeliveryResult<Null, string>? deliveryResult;
        try {

            Message<Null, string> msg = new Message<Null, string> { Value = message };

            if (partition.HasValue) {
                TopicPartition topicPartition = new TopicPartition(topic, partition.Value);
                _logger.LogInformation("Invio  messaggio: {msg}; Verso la TopicPartition: {topicPartition}...", JsonSerializer.Serialize(msg), JsonSerializer.Serialize(topicPartition));
                deliveryResult = await _producer.ProduceAsync(topicPartition, msg, cancellationToken);

            } else {
                _logger.LogInformation("Invio  messaggio: {msg}; Verso il Topic '{topic}'...", JsonSerializer.Serialize(msg), topic);
                deliveryResult = await _producer.ProduceAsync(topic, msg, cancellationToken);

            }


        } catch (ProduceException<Null, string> ex) {
            _logger.LogError(ex, "ProduceException<Null, string> sollevata all'interno del metodo {methodName}: {reason}", nameof(ProduceAsync), ex.Error.Reason);
            throw;
        } catch (KafkaException ex) {
            _logger.LogError(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", nameof(ProduceAsync), ex.Error.Reason);
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Exception sollevata all'interno del metodo {methodName}: {message}", nameof(ProduceAsync), ex.Message);
            throw;
        }

        _logger.LogInformation("Invio  messaggio completato!");
        _logger.LogInformation("deliveryResult: {deliveryResult}", JsonSerializer.Serialize(deliveryResult));
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // Eliminare lo stato gestito (oggetti gestiti)
                try {
                    _producer.Flush(); // Block until all outstanding produce requests have completed (with or without error).
                    _producer.Dispose();
                } catch (KafkaException ex) {
                    _logger.LogCritical(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", nameof(Dispose), ex.Error.Reason);
                    throw;
                } catch (OperationCanceledException ex) {
                    _logger.LogCritical(ex, "OperationCanceledException sollevata all'interno del metodo {methodName}: {message}", nameof(Dispose), ex.Message);
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
    // ~ProducerClient()
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
