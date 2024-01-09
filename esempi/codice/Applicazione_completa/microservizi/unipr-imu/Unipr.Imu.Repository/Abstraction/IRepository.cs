using Imu.Repository.Model;
using Imu.Shared;

namespace Imu.Repository.Abstraction
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CreateCategoriaCatastale(CategoriaCatastaleInsertDto categoriaCatastaleInsertDto, CancellationToken cancellationToken = default);
        Task CreateImmobile(ImmobileInsertDto immobileInsertDto, CancellationToken cancellationToken = default);
        Task AssociaAnagraficaImmobile(AssociaAnagraficaImmobileDto associaAnagraficaImmobileDto, CancellationToken cancellationToken = default);
        Task<List<VersamentoKafka>> GetVersamentiKafka(CancellationToken cancellationToken = default);
        Task InsertVersamentoKafka(VersamentoKafka versamentoKafka, CancellationToken cancellationToken = default);

        #region TransactionalOutbox

        IEnumerable<TransactionalOutbox> GetAllTransactionalOutbox();

        TransactionalOutbox? GetTransactionalOutboxByKey(long id);

        void DeleteTransactionalOutbox(long id);

        void InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox);

        #endregion
    }
}
