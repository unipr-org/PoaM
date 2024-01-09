using AutoMapper;
using ConsumerExample.Repository;
using ConsumerExample.Shared.Dto;
using UniprExample.Client.Http.Abstractions;
using UniprExample.Shared.Dto;

namespace ConsumerExample.Business {
    public class Business : IBusiness {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUniprExampleClient _uniprExampleClient;
        public Business(IRepository repository, IMapper mapper, IUniprExampleClient uniprExampleClient) {
            _repository = repository;
            _mapper = mapper;
            _uniprExampleClient = uniprExampleClient;
        }

        public IEnumerable<StudentiKafkaDto> GetStudentiKafka() {
            return _repository.GetStudentiKafka().Select(_mapper.Map<StudentiKafkaDto>).ToList();
        }

        public async Task<StudenteDto?> GetStudenteByKeyAsync(int matricola, CancellationToken cancellationToken = default) {
            return await _uniprExampleClient.GetStudenteByKeyAsync(matricola, cancellationToken);
        }
    }
}
