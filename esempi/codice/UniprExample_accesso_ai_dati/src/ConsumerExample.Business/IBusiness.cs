using ConsumerExample.Shared.Dto;
using UniprExample.Shared.Dto;

namespace ConsumerExample.Business {
    public interface IBusiness {
        IEnumerable<StudentiKafkaDto> GetStudentiKafka();

        Task<StudenteDto?> GetStudenteByKeyAsync(int matricola, CancellationToken cancellationToken = default);
    }
}
