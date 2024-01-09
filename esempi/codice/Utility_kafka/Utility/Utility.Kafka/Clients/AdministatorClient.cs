using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Utility.Kafka.Abstractions.Clients;

namespace Utility.Kafka.Clients;

public class AdministatorClient : IAdministatorClient {

    readonly IAdminClient _adminClient;
    readonly ILogger<AdministatorClient> _logger;

    bool _disposedValue;

    public AdministatorClient(IOptions<KafkaAdminClientOptions> options, ILogger<AdministatorClient> logger) {
        _logger = logger;
        _adminClient = new AdminClientBuilder(GetAdminClientConfig(options)).Build();
    }

    private AdminClientConfig GetAdminClientConfig(IOptions<KafkaAdminClientOptions> options) {
        AdminClientConfig adminClientConfig = new AdminClientConfig();
        adminClientConfig.BootstrapServers = options.Value.BootstrapServers;
        adminClientConfig.ClientId = Dns.GetHostName();

        _logger.LogInformation("Kafka AdminClientConfig: {adminClientConfig}", JsonSerializer.Serialize(adminClientConfig));
        return adminClientConfig;
    }

    /// <inheritdoc/>
    public void PrintLibraryVersion() {
        _logger.LogInformation("Kafka Library Version: {versionString} ({version}). Debug Contexts: {debugContexts}", Library.VersionString, Library.Version, string.Join(", ", Library.DebugContexts));
    }

    /// <inheritdoc/>
    public async Task<ListConsumerGroupsResult> ListConsumerGroupsAsync() {
        ListConsumerGroupsResult groupsResult = await _adminClient.ListConsumerGroupsAsync();
        _logger.LogInformation("Sono presenti {Count} Kafka Groups", groupsResult.Valid.Count);
        _logger.LogInformation(groupsResult.ToString());
        return groupsResult;
    }

    /// <inheritdoc/>
    public async Task<DescribeConsumerGroupsResult> DescribeConsumerGroupsAsync(IEnumerable<string> groups) {
        DescribeConsumerGroupsResult groupsResult = await _adminClient.DescribeConsumerGroupsAsync(groups);
        _logger.LogInformation(groupsResult.ToString());
        return groupsResult;
    }

    /// <inheritdoc/>
    public Metadata GetMetadata(string? topic = null) {

        /*
         * 
         * Within IAdminClient there is a method GetMetadata(string topic, TimeSpan timeout)
         * which loads the metadata for a given topic.
         * If the topic does not exist, it actually gets created, 
         * instead of an exception being thrown or some other indication that the topic doesn't exist.
         * 
         * It's "by design" in that this is the behavior of the broker. 
         * You can disable this (and it is good practice) with the broker setting:
         * auto.create.topics.enable=false
         * 
         */
        TimeSpan timeSpan = TimeSpan.FromSeconds(30);
        string topicInfo = string.Empty;

        Metadata meta;
        if (string.IsNullOrWhiteSpace(topic)) {
            meta = _adminClient.GetMetadata(timeSpan);
        } else {
            topicInfo = $" per il Topic '{topic}'";
            meta = _adminClient.GetMetadata(topic, timeSpan);
        }

        _logger.LogInformation("Kafka Cluster Metadata{topicInfo}: {meta}", topicInfo, meta.ToString());

        return meta;
    }

    /// <inheritdoc/>
    public bool TopicExists(string topic) => GetMetadata(topic).Topics.Any();

    /// <inheritdoc/>
    public async Task CreateTopicAsync(string topic, short replicationFactor = 1, int numPartitions = 1) {
        _logger.LogInformation("Creazione del topic '{topic}'...", topic);
        await TryCatchAsync(() => _adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = topic, ReplicationFactor = replicationFactor, NumPartitions = numPartitions } }),
                    nameof(CreateTopicAsync));
        _logger.LogInformation("Creazione del '{topic}' completata!", topic);
    }

    /// <inheritdoc/>
    public async Task CreatePartitionsAsync(string topic, int increaseTo = 1, CreatePartitionsOptions? options = null) {
        _logger.LogInformation("Creazione di {increaseTo} partizioni nel topic '{topic}'...", increaseTo, topic);
        await TryCatchAsync(() => _adminClient.CreatePartitionsAsync(new PartitionsSpecification[] {
                    new PartitionsSpecification { Topic = topic, ReplicaAssignments = null, IncreaseTo = increaseTo }}, options),
                    nameof(CreatePartitionsAsync));
        _logger.LogInformation("Creazione di {increaseTo} partizioni nel topic '{topic}' completata!", increaseTo, topic);
    }

    /// <inheritdoc/>
    public async Task DeleteTopicsAsync(IEnumerable<string> topics, DeleteTopicsOptions? options = null) {
        _logger.LogInformation("Cancellazione dei topic '{topics}'...", string.Join("', '", topics));
        await TryCatchAsync(() => _adminClient.DeleteTopicsAsync(topics, options), nameof(DeleteTopicsAsync));
        _logger.LogInformation("Cancellazione dei topic '{topics}' completata!", string.Join("', '", topics));
    }

    private async Task TryCatchAsync(Func<Task> func, string methodName) {
        try {
            await func();
        } catch (CreateTopicsException ex) {
            _logger.LogError(ex, "CreateTopicsException sollevata all'interno del metodo {methodName}: {reason}", methodName, ex.Error.Reason);
            throw;
        } catch (CreatePartitionsException ex) {
            _logger.LogError(ex, "CreatePartitionsException sollevata all'interno del metodo {methodName}: {reason}", methodName, ex.Error.Reason);
            throw;
        } catch (DeleteTopicsException ex) {
            _logger.LogError(ex, "DeleteTopicsException sollevata all'interno del metodo {methodName}: {reason}", methodName, ex.Error.Reason);
            throw;
        } catch (KafkaException ex) {
            _logger.LogError(ex, "KafkaException sollevata all'interno del metodo {methodName}: {reason}", methodName, ex.Error.Reason);
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Exception sollevata all'interno del metodo {methodName}: {message}", methodName, ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // Eliminare lo stato gestito (oggetti gestiti)
                try {
                    _adminClient.Dispose();
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

