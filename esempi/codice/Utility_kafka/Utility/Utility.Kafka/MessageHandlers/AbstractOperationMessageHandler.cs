using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Utility.Kafka.Abstractions.MessageHandlers;
using Utility.Kafka.Constants;
using Utility.Kafka.Exceptions;
using Utility.Kafka.Messages;

namespace Utility.Kafka.MessageHandlers;

/// <summary>
/// Classe per gestire messaggi di tipo <see cref="OperationMessage{TMessageDto}"/>. <br/>
/// Il DTO <typeparamref name="TMessageDto"/> contenuto nel messaggio viene mappato nel DTO di destinazione <typeparamref name="TDomainDto"/>
/// </summary>
/// <typeparam name="TMessageDto">DTO contenuto nel messaggio <see cref="OperationMessage{TMessageDto}"/></typeparam>
/// <typeparam name="TDomainDto">DTO di destinazione</typeparam>
/// <typeparam name="TRepository">Classe utilizzata per eseguire le operazioni di Insert, Update e Delete
/// partendo dal <typeparamref name="TDomainDto"/></typeparam>
public abstract class AbstractOperationMessageHandler<TMessageDto, TDomainDto, TRepository>
    : AbstractOperationMessageHandler<TMessageDto, TRepository>
    where TMessageDto : class, new()
    where TDomainDto : class, new() {

    protected IMapper Mapper { get; }
    protected string DomainDtoType { get; }

    protected AbstractOperationMessageHandler(ILogger<AbstractOperationMessageHandler<TMessageDto, TDomainDto, TRepository>> logger, TRepository repository, IMapper mapper) : base(logger, repository) {
        Mapper = mapper;
        DomainDtoType = typeof(TDomainDto).Name;
    }

    /// <inheritdoc/>
    protected override void Insert(TMessageDto messageDto) {
        Logger.LogInformation("Mapping da {messageDtoType} a {domainDtoType} per eseguire l'Insert...", MessageDtoType, DomainDtoType);
        Insert(Mapper.Map<TDomainDto>(messageDto));
    }

    /// <inheritdoc/>
    protected override void Update(TMessageDto messageDto) {
        Logger.LogInformation("Mapping da {messageDtoType} a {domainDtoType} per eseguire l'Update...", MessageDtoType, DomainDtoType);
        Update(Mapper.Map<TDomainDto>(messageDto));
    }

    /// <inheritdoc/>
    protected override void Delete(TMessageDto messageDto) {
        Logger.LogInformation("Mapping da {messageDtoType} a {domainDtoType} per eseguire la Delete...", MessageDtoType, DomainDtoType);
        Delete(Mapper.Map<TDomainDto>(messageDto));
    }

    /// <summary>
    /// Insert del model partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto"><typeparamref name="TDomainDto"/> utilizzato per inserire il relativo model</param>
    protected abstract void Insert(TDomainDto domainDto);

    /// <summary>
    /// Update del model partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto"><typeparamref name="TDomainDto"/> utilizzato per aggiornare il relativo model</param>
    protected abstract void Update(TDomainDto domainDto);

    /// <summary>
    /// Delete del model partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto"><typeparamref name="TDomainDto"/> utilizzato per eliminare il relativo model</param>
    protected abstract void Delete(TDomainDto domainDto);

}

/// <summary>
/// Classe per gestire messaggi di tipo <see cref="OperationMessage{TMessageDto}"/>.
/// </summary>
/// <typeparam name="TMessageDto">DTO contenuto nel messaggio <see cref="OperationMessage{TMessageDto}"/></typeparam>
/// <typeparam name="TRepository">Classe utilizzata per eseguire le operazioni di Insert, Update e Delete
/// partendo dal <typeparamref name="TMessageDto"/> contenuto nel messaggio</typeparam>
public abstract class AbstractOperationMessageHandler<TMessageDto, TRepository> : IMessageHandler where TMessageDto : class, new() {

    protected ILogger<AbstractOperationMessageHandler<TMessageDto, TRepository>> Logger { get; }
    protected TRepository Repository { get; }
    protected string MessageDtoType { get; }

    protected AbstractOperationMessageHandler(ILogger<AbstractOperationMessageHandler<TMessageDto, TRepository>> logger, TRepository repository) {
        Logger = logger;
        Repository = repository;
        MessageDtoType = typeof(TMessageDto).Name;
    }

    public Task OnMessageReceivedAsync(string msg) {
        Logger.LogInformation("Messaggio OperationMessage da elaborare: '{msg}'...", msg);

        if (string.IsNullOrWhiteSpace(msg)) {
            throw new MessageHandlerException($"Il messaggio {nameof(msg)} {nameof(OperationMessage<TMessageDto>)} non può essere null", nameof(msg));
        }

        return OnMessageReceivedInternalAsync(msg);

    }

    private async Task OnMessageReceivedInternalAsync(string msg) {
        await Task.Factory.StartNew(() => {

            #region Deserializzazione e verifica del messaggio
            Logger.LogInformation("Deserializzazione del messaggio OperationMessage con Dto di tipo {messageDtoType}...", MessageDtoType);

            OperationMessage<TMessageDto>? opMsg;
            try {
                opMsg = JsonSerializer.Deserialize<OperationMessage<TMessageDto>?>(msg);
            } catch (Exception ex) {
                throw new MessageHandlerException($"Si è verificato un errore durante la deserializzazione del messaggio {nameof(msg)} " +
                    $"'{msg}' in {nameof(OperationMessage<TMessageDto>)} con {MessageDtoType} come {nameof(OperationMessage<TMessageDto>.Dto)} : {ex}", nameof(msg), ex);
            }

            if (opMsg == null) {
                throw new MessageHandlerException($"Il {nameof(opMsg)} {nameof(OperationMessage<TMessageDto>)}, " +
                    $"con {MessageDtoType} come {nameof(OperationMessage<TMessageDto>.Dto)}, non può essere null", nameof(msg));
            }

            Logger.LogInformation("Deserializzazione del messaggio eseguita correttamente");

            opMsg.CheckMessage();
            #endregion

            #region Operazione da eseguire
            Logger.LogInformation("Esecuzione operazione '{operation}'...", opMsg.Operation);
            switch (opMsg.Operation) {
                case Operations.Insert:
                    Insert(opMsg.Dto);
                    break;
                case Operations.Update:
                    Update(opMsg.Dto);
                    break;
                case Operations.Delete:
                    Delete(opMsg.Dto);
                    break;
                default:
                    throw new MessageHandlerException($"{nameof(opMsg)}.{nameof(opMsg.Operation)} contiene un valore non valido '{opMsg.Operation}'", $"{nameof(opMsg)}.{nameof(opMsg.Operation)}");
            }
            Logger.LogInformation("Operazione '{operation}' completata con successo", opMsg.Operation);
            #endregion

        });
    }

    /// <summary>
    /// Insert del model partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto"><typeparamref name="TMessageDto"/> utilizzato per inserire il relativo model</param>
    protected abstract void Insert(TMessageDto messageDto);

    /// <summary>
    /// Update del model partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto"><typeparamref name="TMessageDto"/> utilizzato per aggiornare il relativo model</param>
    protected abstract void Update(TMessageDto messageDto);

    /// <summary>
    /// Delete del model partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto"><typeparamref name="TMessageDto"/> utilizzato per eliminare il relativo model</param>
    protected abstract void Delete(TMessageDto messageDto);

}
