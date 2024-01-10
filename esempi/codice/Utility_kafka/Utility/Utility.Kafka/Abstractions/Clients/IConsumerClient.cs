using Confluent.Kafka;

namespace Utility.Kafka.Abstractions.Clients;

public interface IConsumerClient : IDisposable {

    /// <summary>
    /// Restituisce le sottoscrizioni presenti
    /// </summary>
    /// <returns></returns>
    List<string> GetCurrentSubscription();

    /// <summary>
    /// Sottoscrizione ad una lista di <paramref name="topics"/>
    /// </summary>
    /// <param name="topics">Topics a cui sottoscriversi</param>
    void Subscribe(IEnumerable<string> topics);

    /// <summary>
    /// Sottoscrizione ad un <paramref name="topic"/>
    /// </summary>
    /// <param name="topic">Topic a cui sottoscriversi</param>
    void Subscribe(string topic);

    /// <summary>
    /// Reset delle sottoscrizioni
    /// </summary>
    void Unsubscribe();

    /// <summary>
    /// Lettura di nuovi messaggi/eventi. <br/>
    /// Il metodo rimane in attesa sul bus finchè trova un messaggio, <br/>
    /// inoltre viene ripetuta la lettura finche non è stata richiesta la cancellazione del token <paramref name="cancellationToken"/>. <br/>
    /// </summary>
    /// <param name="cancellationToken">CancellationToken per terminare l'operazione</param>
    /// <returns></returns>
    Task<ConsumeResult<Null, string>> ConsumeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Per prima cosa viene esegue la sottoscrizione ad un <paramref name="topic"/> (tramite il metodo <b>Subscribe</b>). <br/>
    /// Successivamente avviene la lettura di nuovi messaggi/eventi (tramite il metodo <b>ConsumeAsync</b>). <br/>
    /// Il metodo rimane in attesa sul bus finchè trova un messaggio, <br/>
    /// inoltre viene ripetuta la lettura finche non è stata richiesta la cancellazione del token <paramref name="cancellationToken"/>. <br/>
    /// Infine, il messaggio recuperato viene passato in input alla Action <paramref name="comsumerOperationsAsync"/> che lo elaborerà. <br/>
    /// Completata l'elaborazione, anche in caso di errore, viene chiamato il metodo <see cref="Commit"/>.
    /// </summary>
    /// <param name="topic">Topic a cui sottoscriversi</param>
    /// <param name="comsumerOperationsAsync">Func Async che si occupa di elaborare il messaggio recuperato</param>
    /// <param name="cancellationToken">CancellationToken per terminare l'operazione</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    Task<bool> ConsumeInLoopAsync(string topic, Func<ConsumeResult<Null, string>, Task> comsumerOperationsAsync, CancellationToken cancellationToken = default);

    /// <summary>
    /// Per prima cosa viene esegue la sottoscrizione ad una lista di <paramref name="topics"/> (tramite il metodo <b>Subscribe</b>). <br/>
    /// Successivamente avviene la lettura di nuovi messaggi/eventi (tramite il metodo <b>ConsumeAsync</b>). <br/>
    /// Il metodo rimane in attesa sul bus finchè trova un messaggio, <br/>
    /// inoltre viene ripetuta la lettura finche non è stata richiesta la cancellazione del token <paramref name="cancellationToken"/>. <br/>
    /// Infine, il messaggio recuperato viene passato in input alla Action <paramref name="comsumerOperationsAsync"/> che lo elaborerà. <br/>
    /// Completata l'elaborazione, anche in caso di errore, viene chiamato il metodo <see cref="Commit"/>.
    /// </summary>
    /// <param name="topics">Lista di topic a cui sottoscriversi</param>
    /// <param name="comsumerOperationsAsync">Func Async che si occupa di elaborare il messaggio recuperato</param>
    /// <param name="cancellationToken">CancellationToken per terminare l'operazione</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    Task<bool> ConsumeInLoopAsync(IEnumerable<string> topics, Func<ConsumeResult<Null, string>, Task> comsumerOperationsAsync, CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit manuale di un offset relativo ad un topic/partition/offset di un <see cref="ConsumeResult"/>. <br/>
    /// Da utilizzare se viene disabilitato il <see cref="ConsumerConfig.EnableAutoCommit"/>
    /// </summary>
    /// <param name="result">Istanza <see cref="ConsumeResult"/> usata per determinare l'offset su cui fare commit</param>
    void Commit(ConsumeResult<Null, string>? result);
}

