using Pagamenti.Shared;

namespace Pagamenti.Business.Abstraction
{
    public interface IBusiness
    {
        Task CreateVersamento(VersamentoInsertDto versamentoInsertDto, CancellationToken cancellationToken = default);
    }
}
