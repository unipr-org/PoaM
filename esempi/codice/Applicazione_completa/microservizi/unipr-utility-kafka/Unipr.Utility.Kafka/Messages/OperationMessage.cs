using Utility.Kafka.Abstractions.Messages;
using Utility.Kafka.Constants;
using Utility.Kafka.Exceptions;

namespace Utility.Kafka.Messages;

public class OperationMessage<TDto> : IOperationMessage<TDto> where TDto : class, new() {
    /// <summary>
    /// Operazione da eseguire. <br/>
    /// Dominio valori: <see cref="Operations"/>
    /// </summary>
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// Dto da elaborare
    /// </summary>
    public TDto Dto { get; set; } = new();

    /// <summary>
    /// Verifica se il messaggio è valorizzato correttamente
    /// </summary>
    public void CheckMessage() {
        if (string.IsNullOrWhiteSpace(Operation)) {
            throw new MessageException($"La property {nameof(Operation)} non può essere null", nameof(Operation));
        }
        if (!Operations.IsValid(Operation)) {
            throw new MessageException($"La property {nameof(Operation)} contiene un valore non valido '{Operation}'", nameof(Operation));
        }

        if (Dto == null) {
            throw new MessageException($"La property {nameof(Dto)} ({typeof(TDto).Name}) non può essere null", nameof(Dto));
        }
    }
}
