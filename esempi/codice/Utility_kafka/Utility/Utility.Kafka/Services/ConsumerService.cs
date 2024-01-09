using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.Kafka.Abstractions.Clients;
using Utility.Kafka.Abstractions.MessageHandlers;

namespace Utility.Kafka.Services;

public class ConsumerService<TKafkaTopicsInput> : BackgroundService where TKafkaTopicsInput : class, IKafkaTopics {
    protected ILogger<ConsumerService<TKafkaTopicsInput>> Logger { get; }
    protected IConsumerClient ConsumerClient { get; }
    protected IAdministatorClient AdminClient { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IMessageHandlerFactory MessageHandlerFactory { get; }
    protected IEnumerable<string> Topics { get; }

    bool _disposedValue;

    public ConsumerService(ILogger<ConsumerService<TKafkaTopicsInput>> logger, IConsumerClient consumerClient, IAdministatorClient adminClient, IOptions<TKafkaTopicsInput> optionsTopics, IServiceScopeFactory serviceScopeFactory, IMessageHandlerFactory messageHandlerFactory) {
        Logger = logger;
        ConsumerClient = consumerClient;
        AdminClient = adminClient;
        Topics = optionsTopics.Value.GetTopics();
        ServiceScopeFactory = serviceScopeFactory;
        MessageHandlerFactory = messageHandlerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        Logger.LogInformation("START ConsumerService.ExecuteAsync...");

        foreach (string topic in Topics) {
            if (!AdminClient.TopicExists(topic)) {
                await AdminClient.CreateTopicAsync(topic);
            }
        }

        await ConsumerClient.ConsumeInLoopAsync(Topics, async msg => {
            using IServiceScope scope = ServiceScopeFactory.CreateScope();
            IMessageHandler handler = MessageHandlerFactory.Create(msg.Topic, scope.ServiceProvider);
            await handler.OnMessageReceivedAsync(msg.Message.Value);
        }, stoppingToken);

        Logger.LogInformation("STOP ConsumerService");

    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // Eliminare lo stato gestito (oggetti gestiti)
                ConsumerClient?.Dispose();
                AdminClient?.Dispose();
                base.Dispose();
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
    public override void Dispose() {
        // Non modificare questo codice. Inserire il codice di pulizia nel metodo 'Dispose(bool disposing)'
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}

