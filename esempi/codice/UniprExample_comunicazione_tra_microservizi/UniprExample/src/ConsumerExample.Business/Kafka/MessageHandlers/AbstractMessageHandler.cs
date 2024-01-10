using AutoMapper;
using ConsumerExample.Repository;
using Microsoft.Extensions.Logging;
using Utility.Kafka.MessageHandlers;

namespace ConsumerExample.Business.Kafka.MessageHandlers;

public abstract class AbstractMessageHandler<TMessageDTO>
    : AbstractOperationMessageHandler<TMessageDTO, IRepository>
    where TMessageDTO : class, new() {

    protected AbstractMessageHandler(ILogger<AbstractMessageHandler<TMessageDTO>> logger, IRepository repository) : base(logger, repository) { }

    /// <summary>
    /// Insert del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da inserire</param>
    protected override void Insert(TMessageDTO messageDto) {
        Logger.LogInformation("Insert del DTO {messageDTOType}...", MessageDtoType);
        InsertDto(messageDto);
        Repository.SaveChanges();
        Logger.LogInformation("Insert del DTO {messageDTOType} completata!", MessageDtoType);
    }

    /// <summary>
    /// Update del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da aggiornare</param>
    protected override void Update(TMessageDTO messageDto) {
        Logger.LogInformation("Update del DTO {messageDTOType}...", MessageDtoType);
        UpdateDto(messageDto);
        Repository.SaveChanges();
        Logger.LogInformation("Update del DTO {messageDTOType} completata", MessageDtoType);
    }

    /// <summary>
    /// Delete del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da eliminare</param>
    protected override void Delete(TMessageDTO messageDto) {
        Logger.LogInformation("Delete del DTO {messageDTOType}...", MessageDtoType);
        DeleteDto(messageDto);
        Repository.SaveChanges();
        Logger.LogInformation("Delete del DTO {messageDTOType} completata", MessageDtoType);
    }

    /// <summary>
    /// Insert del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da inserire</param>
    protected abstract void InsertDto(TMessageDTO messageDto);

    /// <summary>
    /// Update del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da aggiornare</param>
    protected abstract void UpdateDto(TMessageDTO messageDto);

    /// <summary>
    /// Delete del record partendo dal <paramref name="messageDto"/>
    /// </summary>
    /// <param name="messageDto">Dto del record da eliminare</param>
    protected abstract void DeleteDto(TMessageDTO messageDto);
}

public abstract class AbstractMessageHandler<TMessageDTO, TDomainDTO>
    : AbstractOperationMessageHandler<TMessageDTO, TDomainDTO, IRepository>
    where TMessageDTO : class, new()
    where TDomainDTO : class, new() {

    protected AbstractMessageHandler(ILogger<AbstractMessageHandler<TMessageDTO, TDomainDTO>> logger, IRepository repository, IMapper map) : base(logger, repository, map) { }

    /// <summary>
    /// Insert del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da inserire</param>
    protected override void Insert(TDomainDTO domainDto) {
        Logger.LogInformation("Insert del DTO {domainDTOType}...", DomainDtoType);
        InsertDto(domainDto);
        Repository.SaveChanges();
        Logger.LogInformation("Insert del DTO {domainDTOType} completata!", DomainDtoType);
    }

    /// <summary>
    /// Update del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da aggiornare</param>
    protected override void Update(TDomainDTO domainDto) {
        Logger.LogInformation("Update del DTO {domainDTOType}...", DomainDtoType);
        UpdateDto(domainDto);
        Repository.SaveChanges();
        Logger.LogInformation("Update del DTO {domainDTOType} completata", DomainDtoType);
    }

    /// <summary>
    /// Delete del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da eliminare</param>
    protected override void Delete(TDomainDTO domainDto) {
        Logger.LogInformation("Delete del DTO {domainDTOType}...", DomainDtoType);
        DeleteDto(domainDto);
        Repository.SaveChanges();
        Logger.LogInformation("Delete del DTO {domainDTOType} completata", DomainDtoType);
    }

    /// <summary>
    /// Insert del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da inserire</param>
    protected abstract void InsertDto(TDomainDTO domainDto);

    /// <summary>
    /// Update del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da aggiornare</param>
    protected abstract void UpdateDto(TDomainDTO domainDto);

    /// <summary>
    /// Delete del record partendo dal <paramref name="domainDto"/>
    /// </summary>
    /// <param name="domainDto">Dto del record da eliminare</param>
    protected abstract void DeleteDto(TDomainDTO domainDto);
}
