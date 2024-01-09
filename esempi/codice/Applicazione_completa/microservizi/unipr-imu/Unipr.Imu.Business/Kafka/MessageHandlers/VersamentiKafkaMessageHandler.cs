using AutoMapper;
using Imu.Repository.Abstraction;
using Imu.Repository.Model;
using Microsoft.Extensions.Logging;
using Pagamenti.Business.Kafka.MessageHandlers;
using Pagamenti.Shared;

namespace Imu.Business.Kafka.MessageHandlers;

public class VersamentiKafkaMessageHandler : AbstractMessageHandler<VersamentoReadDto, VersamentoKafka> {
    public VersamentiKafkaMessageHandler(ILogger<VersamentiKafkaMessageHandler> logger, IRepository repository, IMapper map) : base(logger, repository, map) { }

    protected override async Task InsertDtoAsync(VersamentoKafka domainDto, CancellationToken cancellationToken = default) {
        await Repository.InsertVersamentoKafka(domainDto, cancellationToken);
    }
    protected override async Task UpdateDtoAsync(VersamentoKafka domainDto, CancellationToken cancellationToken = default) {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
    protected override async Task DeleteDtoAsync(VersamentoKafka domainDto, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
