namespace Utility.Kafka.Abstractions.Clients;

public interface IProducerClient : IDisposable {

    /// <summary>
    /// Invio di un <paramref name="message"/> ad un <paramref name="topic"/>; la partizione è determinata dalla configurazione
    /// </summary>
    /// <param name="topic">Topic a cui inviare il messaggio</param>
    /// <param name="message">Messaggio da inviare</param>
    /// <param name="cancellationToken">CancellationToken per terminare l'operazione</param>
    /// <returns></returns>
    Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invio di un <paramref name="message"/> ad un <paramref name="topic"/> su una specifica <paramref name="partition"/>
    /// </summary>
    /// <param name="topic">Topic a cui inviare il messaggio</param>
    /// <param name="partition">Partizione del topic</param>
    /// <param name="message">Messaggio da inviare</param>
    /// <param name="cancellationToken">CancellationToken per terminare l'operazione</param>
    /// <returns></returns>
    Task ProduceAsync(string topic, int partition, string message, CancellationToken cancellationToken = default);

}
