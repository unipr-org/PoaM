using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UniprExample.Repository.Model;

namespace UniprExample.Repository {
    public class Repository : IRepository {
        private readonly UniprExampleDbContext _uniprExampleDbContext;
        public Repository(UniprExampleDbContext uniprExampleDbContext) {
            _uniprExampleDbContext = uniprExampleDbContext;
        }

        public void SaveChanges() => _uniprExampleDbContext.SaveChanges();

        public IDbContextTransaction BeginTRansaction() => _uniprExampleDbContext.Database.BeginTransaction();

        public void CreateTransaction(Action action) {
            if (_uniprExampleDbContext.Database.CurrentTransaction != null) {
                // La connessione è già in una transazione
                action();
            } else {
                // Viene avviata una transazione 
                using IDbContextTransaction transaction = _uniprExampleDbContext.Database.BeginTransaction();
                try {
                    action();
                    transaction.Commit();
                } catch (Exception) {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Studenti Create(string cognome, string nome, DateTime dataDiNascita) {
            var s = new Studenti() {
                Cognome = cognome,
                Nome = nome,
                DataDiNascita = dataDiNascita
            };

            _uniprExampleDbContext.Studenti.Add(s);

            //_uniprDbContext.SaveChanges();

            return s;
        }

        public Studenti? Read(int matricola) {
            return _uniprExampleDbContext.Studenti.SingleOrDefault(p => p.Matricola == matricola);
        }

        public IQueryable<Studenti> Read() {
            return _uniprExampleDbContext.Studenti;
        }

        public IQueryable<Studenti> Read(string cognome, string nome) {
            return _uniprExampleDbContext.Studenti.Where(p => p.Cognome == cognome && p.Nome == nome);
        }

        public Studenti? GetStudenteByKey(int matricola) {
            return _uniprExampleDbContext.Studenti.FirstOrDefault(p => p.Matricola == matricola);
        }

        public Studenti Update(Studenti studente, string cognome, string nome, DateTime dataDiNascita) {
            studente.Cognome = cognome;
            studente.Nome = nome;
            studente.DataDiNascita = dataDiNascita;

            _uniprExampleDbContext.Update(studente);

            //_uniprDbContext.SaveChanges();

            return studente;
        }

        public Studenti? Update(int matricola, string cognome, string nome, DateTime dataDiNascita) {
            var studente = _uniprExampleDbContext.Studenti.Where(p => p.Matricola == matricola).SingleOrDefault();

            if (studente is null)
                return null;

            studente.Cognome = cognome;
            studente.Nome = nome;
            studente.DataDiNascita = dataDiNascita;

            _uniprExampleDbContext.Update(studente);

            //_uniprDbContext.SaveChanges();

            return studente;
        }

        public void Delete(int matricola) {
            var studente = _uniprExampleDbContext.Studenti.SingleOrDefault(p => p.Matricola == matricola);

            if (studente is null)
                return;

            _uniprExampleDbContext.Studenti.Remove(studente);
        }

        public void Delete(Studenti? studente) {
            if (studente is null)
                return;

            _uniprExampleDbContext.Studenti.Remove(studente);
        }

        public void Delete(IEnumerable<Studenti> studenti) {
            _uniprExampleDbContext.Studenti.RemoveRange(studenti);
        }

        public void Delete(string cognome, string nome) {
            var studenti = _uniprExampleDbContext.Studenti.Where(p => p.Cognome == cognome && p.Nome == nome).ToList();

            foreach (var s in studenti) {
                _uniprExampleDbContext.Studenti.Remove(s);
            }

            // alternativa
            //_uniprDbContext.Studenti.RemoveRange(studenti);
        }

        public int ExecuteUpdate(string cognome) {
            return _uniprExampleDbContext.Studenti.Where(p => p.Cognome.StartsWith("a")).ExecuteUpdate(p => p.SetProperty(s => s.Cognome, cognome));
        }

        public int ExecuteDelete(string cognome) {
            return _uniprExampleDbContext.Studenti.Where(p => p.Cognome == cognome).ExecuteDelete();
        }

        public IQueryable<Tuple<Studenti, Corsi, Esami>> GetEsamiStudenti(int corsoId) {
            return _uniprExampleDbContext
                .Esami
                .Include(p => p.Studente)
                .Include(p => p.Corso)
                .Where(p => p.CorsiId == corsoId)
                .Select(p => new Tuple<Studenti, Corsi, Esami>(p.Studente, p.Corso, p));
        }

        public async Task<List<Tuple<Studenti, Corsi, Esami>>> GetEsamiStudentiAsync(int corsoId, CancellationToken cancellationToken = default) {
            return await _uniprExampleDbContext
                .Esami
                .Include(p => p.Studente)
                .Include(p => p.Corso)
                .Where(p => p.CorsiId == corsoId)
                .Select(p => new Tuple<Studenti, Corsi, Esami>(p.Studente, p.Corso, p))
                .ToListAsync(cancellationToken);
        }

        public void InsertStudente(Studenti studente) {
            _uniprExampleDbContext.Studenti.Add(studente);
        }

        #region TransactionalOutbox


        public IEnumerable<TransactionalOutbox> GetAllTransactionalOutbox() => _uniprExampleDbContext.TransactionalOutboxList.ToList();

        public TransactionalOutbox? GetTransactionalOutboxByKey(long id) {

            return _uniprExampleDbContext.TransactionalOutboxList.FirstOrDefault(x =>
                x.Id == id);
        }

        public void DeleteTransactionalOutbox(long id) {

            _uniprExampleDbContext.TransactionalOutboxList.Remove(
                GetTransactionalOutboxByKey(id) ??
                throw new ArgumentException($"TransactionalOutbox con id {id} non trovato", nameof(id)));
        }

        public void InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox) {
            _uniprExampleDbContext.TransactionalOutboxList.Add(transactionalOutbox);
        }

        #endregion

    }
}
