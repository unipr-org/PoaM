using Pagamenti.Repository.Model;
using Pagamenti.Shared;

namespace Pagamenti.Repository.Abstraction
{
    public interface IRepository
    {
        Task CreateTransaction(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Versamento> CreateVersamento(VersamentoInsertDto versamentoInsertDto, CancellationToken cancellationToken = default);


        #region TransactionalOutbox

        Task<IEnumerable<TransactionalOutbox>> GetAllTransactionalOutbox(CancellationToken cancellationToken = default);

        Task<TransactionalOutbox?> GetTransactionalOutboxByKey(long id, CancellationToken cancellationToken = default);

        Task DeleteTransactionalOutbox(long id, CancellationToken cancellationToken = default);

        Task InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox, CancellationToken cancellationToken = default);

        #endregion
    }
}
