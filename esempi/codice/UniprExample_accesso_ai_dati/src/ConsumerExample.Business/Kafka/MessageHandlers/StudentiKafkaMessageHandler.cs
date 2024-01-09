using AutoMapper;
using ConsumerExample.Repository;
using ConsumerExample.Repository.Model;
using Microsoft.Extensions.Logging;
using UniprExample.Shared.Dto;

namespace ConsumerExample.Business.Kafka.MessageHandlers;

public class StudentiKafkaMessageHandler : AbstractMessageHandler<StudenteDto, StudentiKafka> {
    public StudentiKafkaMessageHandler(ILogger<StudentiKafkaMessageHandler> logger, IRepository repository, IMapper map) : base(logger, repository, map) { }

    protected override void InsertDto(StudentiKafka domainDto) {
        Repository.InsertStudentiKafka(domainDto);
    }
    protected override void UpdateDto(StudentiKafka domainDto) {
        throw new NotImplementedException();
    }
    protected override void DeleteDto(StudentiKafka domainDto) {
        throw new NotImplementedException();
    }
}
