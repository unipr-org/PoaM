using UniprExample.Shared.Dto;

namespace UniprExample.Client.Http.Abstractions;
public interface IUniprExampleClient {

    Task<StudenteDto?> GetStudenteByKeyAsync(int matricola, CancellationToken cancellationToken = default);

}
