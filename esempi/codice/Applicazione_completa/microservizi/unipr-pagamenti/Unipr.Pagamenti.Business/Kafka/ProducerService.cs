using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pagamenti.Repository.Abstraction;
using Pagamenti.Repository.Model;
using Utility.Kafka.Abstractions.Clients;
using Utility.Kafka.Services;

namespace Pagamenti.Business.Kafka;
public class ProducerService : ProducerService<KafkaTopicsOutput> {

    public ProducerService(ILogger<ProducerService> logger, IProducerClient producerClient, IAdministatorClient adminClient, IOptions<KafkaTopicsOutput> optionsTopics, IOptions<KafkaProducerServiceOptions> optionsProducerService, IServiceScopeFactory serviceScopeFactory) : base(logger, producerClient, adminClient, optionsTopics, optionsProducerService, serviceScopeFactory) {
    }

    protected override async Task OperationsAsync(CancellationToken cancellationToken) {
        using IServiceScope scope = ServiceScopeFactory.CreateScope();
        IRepository repository = scope.ServiceProvider.GetRequiredService<IRepository>();

        Logger.LogInformation("Acquisizione dei TransactionalOutbox da elaborare...");
        IEnumerable<TransactionalOutbox> transactionalOutboxList = (await repository.GetAllTransactionalOutbox(cancellationToken)).OrderBy(x => x.Id);
        if (!transactionalOutboxList.Any()) {
            Logger.LogInformation($"Non ci sono TransactionalOutbox da elaborare");
            return;
        }

        Logger.LogInformation("Ci sono {Count} TransactionalOutbox da elaborare", transactionalOutboxList.Count());

        foreach (TransactionalOutbox tran in transactionalOutboxList) {
            string groupMsg = $"del record {nameof(TransactionalOutbox)} con " +
                    $"{nameof(TransactionalOutbox.Id)} = {tran.Id}, " +
                    $"{nameof(TransactionalOutbox.Tabella)} = '{tran.Tabella}' e " +
                    $"{nameof(TransactionalOutbox.Messaggio)} = '{tran.Messaggio}'";

            Logger.LogInformation("Elaborazione {groupMsg}...", groupMsg);

            try {

                Logger.LogInformation("Determinazione del topic...");
                string topic = tran.Tabella switch {
                    nameof(Versamento) => KafkaTopics.Versamenti,
                    _ => throw new ArgumentOutOfRangeException($"La tabella {tran.Tabella} non è prevista come topic nel Producer")
                };

                Logger.LogInformation("Scrittura del messaggio Kafka sul topic '{topic}'...", topic);
                await ProducerClient.ProduceAsync(topic, tran.Messaggio, cancellationToken);

                Logger.LogInformation("Eliminazione {groupMsg}...", groupMsg);
                await repository.DeleteTransactionalOutbox(tran.Id, cancellationToken);

                await repository.SaveChangesAsync(cancellationToken);
                Logger.LogInformation("Record eliminato");

            } catch (Exception ex) {
                Logger.LogError(ex, "Si è verificata un'eccezione nel metodo ProducerService.OperationsAsync durante l'elaborazione {groupMsg}: {ex}", groupMsg, ex);
            }
        }
    }
}
