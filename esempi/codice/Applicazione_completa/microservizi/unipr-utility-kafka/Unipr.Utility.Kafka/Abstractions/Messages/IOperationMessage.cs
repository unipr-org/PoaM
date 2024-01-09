using Utility.Kafka.Constants;

namespace Utility.Kafka.Abstractions.Messages;

public interface IOperationMessage<TDto> where TDto : class, new() {
    /// <summary>
    /// Operazione da eseguire. <br/>
    /// Dominio valori: <see cref="Operations"/>
    /// </summary>
    string Operation { get; set; }

    /// <summary>
    /// Dto da elaborare
    /// </summary>
    TDto Dto { get; set; }

    /// <summary>
    /// Verifica se il messaggio è valorizzato correttamente
    /// </summary>
    void CheckMessage();
}
