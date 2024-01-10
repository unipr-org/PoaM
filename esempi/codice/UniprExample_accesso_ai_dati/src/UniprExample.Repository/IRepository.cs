using Microsoft.EntityFrameworkCore.Storage;
using UniprExample.Repository.Model;

namespace UniprExample.Repository {
    public interface IRepository {
        void SaveChanges();

        IDbContextTransaction BeginTRansaction();

        void CreateTransaction(Action action);

        Studenti Create(string cognome, string nome, DateTime dataDiNascita);

        IQueryable<Studenti> Read();

        IQueryable<Studenti> Read(string cognome, string nome);

        Studenti? GetStudenteByKey(int matricola);

        Studenti Update(Studenti studente, string cognome, string nome, DateTime dataDiNascita);

        Studenti? Update(int matricola, string cognome, string nome, DateTime dataDiNascita);

        void Delete(int matricola);

        void Delete(Studenti? studente);

        void Delete(IEnumerable<Studenti> studenti);

        void Delete(string cognome, string nome);

        int ExecuteUpdate(string cognome);

        int ExecuteDelete(string cognome);

        IQueryable<Tuple<Studenti, Corsi, Esami>> GetEsamiStudenti(int corsoId);

        void InsertStudente(Studenti studente);

        #region TransactionalOutbox

        IEnumerable<TransactionalOutbox> GetAllTransactionalOutbox();

        TransactionalOutbox? GetTransactionalOutboxByKey(long id);

        void DeleteTransactionalOutbox(long id);

        void InsertTransactionalOutbox(TransactionalOutbox transactionalOutbox);

        #endregion

    }
}
