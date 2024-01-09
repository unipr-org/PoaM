using Imu.Repository.Abstraction;
using Imu.Repository.Model;
using Imu.Shared;
using Microsoft.EntityFrameworkCore;

namespace Imu.Repository
{
    public class Repository : IRepository
    {
        private ImuDbContext _imuDbContext;
        public Repository(ImuDbContext imuDbContext)
        {
            _imuDbContext = imuDbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _imuDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateCategoriaCatastale(CategoriaCatastaleInsertDto categoriaCatastaleInsertDto, CancellationToken cancellationToken = default)
        {
            CategoriaCatastale categoriaCatastale = new CategoriaCatastale()
            {
                Codice = categoriaCatastaleInsertDto.Codice,
                Descrizione = categoriaCatastaleInsertDto.Descrizione,
            };

            await _imuDbContext.CategorieCatastali.AddAsync(categoriaCatastale, cancellationToken);
        }

        public async Task CreateImmobile(ImmobileInsertDto immobileInsertDto, CancellationToken cancellationToken = default)
        {
            Immobile immobile = new Immobile()
            {
                IdCategoriaCatastale = immobileInsertDto.IdCategoriaCatastale,
                Superficie = immobileInsertDto.Superficie,
                Rendita = immobileInsertDto.Rendita,
                Sezione = immobileInsertDto.Sezione,
                Foglio = immobileInsertDto.Foglio,
                Particella = immobileInsertDto.Particella,
                Subalterno = immobileInsertDto.Subalterno,
                Indirizzo = immobileInsertDto.Indirizzo,
                NumeroCivico = immobileInsertDto.NumeroCivico,
                Cap = immobileInsertDto.Cap,
                Provincia = immobileInsertDto.Provincia,
                Localita = immobileInsertDto.Localita,
            };

            await _imuDbContext.Immobili.AddAsync(immobile, cancellationToken);
        }

        public async Task AssociaAnagraficaImmobile(AssociaAnagraficaImmobileDto associaAnagraficaImmobileDto, CancellationToken cancellationToken = default)
        {
            AnagraficaImmobile anagraficaImmobile = new AnagraficaImmobile()
            {
                IdAnagrafica = associaAnagraficaImmobileDto.IdAnagrafica,
                IdImmobile = associaAnagraficaImmobileDto.IdImmobile,
                Quota = associaAnagraficaImmobileDto.Quota
            };

            await _imuDbContext.AnagraficheImmobili.AddAsync(anagraficaImmobile, cancellationToken);
        }

        public async Task<List<VersamentoKafka>> GetVersamentiKafka(CancellationToken cancellationToken = default)
        {
            return await _imuDbContext.VersamentiKafka.ToListAsync(cancellationToken);
        }

        public async Task InsertVersamentoKafka(VersamentoKafka versamentoKafka, CancellationToken cancellationToken = default)
        {
            await _imuDbContext.VersamentiKafka.AddAsync(versamentoKafka, cancellationToken);
        }

        #region TransactionalOutbox


        public IEnumerable<TransactionalOutbox> GetAllTransactionalOutbox() => _imuDbContext.TransactionalOutboxList.ToList();

        public TransactionalOutbox? GetTransactionalOutboxByKey(long id)
        {

            return _imuDbContext.TransactionalOutboxList.FirstOrDefault(x =>
                x.Id == id);
        }

        public void DeleteTransactionalOutbox(long id)
        {

            _imuDbContext.TransactionalOutboxList.Remove(
                GetTransactionalOutboxByKey(id) ??
                throw new ArgumentException($"TransactionalOutbox con id {id} non trovato", nameof(id)));
        }

        public void InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox)
        {
            _imuDbContext.TransactionalOutboxList.Add(transactionalOutbox);
        }

        #endregion
    }
}
