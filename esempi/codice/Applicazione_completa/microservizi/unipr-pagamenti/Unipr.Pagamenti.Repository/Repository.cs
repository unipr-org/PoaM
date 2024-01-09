using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Pagamenti.Repository.Abstraction;
using Pagamenti.Repository.Model;
using Pagamenti.Shared;

namespace Pagamenti.Repository
{
    public class Repository : IRepository
    {
        private PagamentiDbContext _anagraficeDbContext;
        public Repository(PagamentiDbContext anagraficheDbContext)
        {
            _anagraficeDbContext = anagraficheDbContext;
        }

        public async Task CreateTransaction(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            if (_anagraficeDbContext.Database.CurrentTransaction != null)
            {
                // La connessione è già in una transazione
                await action(cancellationToken);
            }
            else
            {
                // Viene avviata una transazione 
                using IDbContextTransaction transaction = await _anagraficeDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _anagraficeDbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Versamento> CreateVersamento(VersamentoInsertDto versamentoInsertDto, CancellationToken cancellationToken = default)
        {
            Versamento versamento = new Versamento();
            versamento.DataVersamento = versamentoInsertDto.DataVersamento;
            versamento.DataContabile = versamentoInsertDto.DataContabile;
            versamento.Importo = versamentoInsertDto.Importo;
            versamento.Informazioni = versamentoInsertDto.Informazioni;

            await _anagraficeDbContext.Versamenti.AddAsync(versamento, cancellationToken);

            return versamento;
        }

        #region TransactionalOutbox


        public async Task<IEnumerable<TransactionalOutbox>> GetAllTransactionalOutbox(CancellationToken cancellationToken = default) => await _anagraficeDbContext.TransactionalOutboxList.ToListAsync(cancellationToken);

        public async Task<TransactionalOutbox?> GetTransactionalOutboxByKey(long id, CancellationToken cancellationToken = default)
        {
            return await _anagraficeDbContext.TransactionalOutboxList.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task DeleteTransactionalOutbox(long id, CancellationToken cancellationToken = default)
        {
            _anagraficeDbContext.TransactionalOutboxList.Remove(
                (await GetTransactionalOutboxByKey(id, cancellationToken)) ??
                throw new ArgumentException($"TransactionalOutbox con id {id} non trovato", nameof(id)));
        }

        public async Task InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox, CancellationToken cancellationToken = default)
        {
            await _anagraficeDbContext.TransactionalOutboxList.AddAsync(transactionalOutbox);
        }

        #endregion
    }
}
