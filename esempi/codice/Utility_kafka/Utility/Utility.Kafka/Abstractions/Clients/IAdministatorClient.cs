using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Utility.Kafka.Abstractions.Clients;

public interface IAdministatorClient : IDisposable {

    /// <summary>
    /// Stampa a log la versione della Kafka Library
    /// </summary>
    void PrintLibraryVersion();

    /// <summary>
    /// Restituisce la lista dei <see cref="ConsumerGroupListing"/> relativa i gruppi presenti
    /// </summary>
    /// <returns></returns>
    Task<ListConsumerGroupsResult> ListConsumerGroupsAsync();

    /// <summary>
    /// Ritorna la lista dei <see cref="ConsumerGroupDescription"/> relativa i gruppi <paramref name="groups"/> specificati in input.
    /// </summary>
    /// <param name="groups"></param>
    /// <returns></returns>
    Task<DescribeConsumerGroupsResult> DescribeConsumerGroupsAsync(IEnumerable<string> groups);

    /// <summary>
    /// Restituisce i metadati relativi al cluster. <br/>
    /// È possibile specificare il parametro opzionale <paramref name="topic"/>
    /// </summary>
    /// <param name="topic">Parametro opzionale</param>
    /// <returns></returns>
    Metadata GetMetadata(string? topic = null);

    /// <summary>
    /// Verifica la presenza di un <paramref name="topic"/>
    /// </summary>
    /// <param name="topic">Nome del topic</param>
    /// <returns></returns>
    bool TopicExists(string topic);


    /// <summary>
    /// Crea un nuovo <paramref name="topic"/>
    /// </summary>
    /// <param name="topic">Nome del topic</param>
    /// <param name="replicationFactor"></param>
    /// <param name="numPartitions"></param>
    /// <returns></returns>
    Task CreateTopicAsync(string topic, short replicationFactor = 1, int numPartitions = 1);

    /// <summary>
    /// Crea una o più partizioni in un determinato <paramref name="topic"/>
    /// </summary>
    /// <param name="topic">Topic in cui creare le partizioni</param>
    /// <param name="increaseTo">Numero di partizioni da creare. <br/>
    /// Di default viene creata una sola nuova partizione</param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task CreatePartitionsAsync(string topic, int increaseTo = 1, CreatePartitionsOptions? options = null);

    /// <summary>
    /// Elimina un elenco di topic
    /// </summary>
    /// <param name="topics">Elenco di topic da eliminare</param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task DeleteTopicsAsync(IEnumerable<string> topics, DeleteTopicsOptions? options = null);

}
