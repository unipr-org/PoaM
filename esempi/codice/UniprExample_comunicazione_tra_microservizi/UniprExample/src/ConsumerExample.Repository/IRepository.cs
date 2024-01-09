using ConsumerExample.Repository.Model;

namespace ConsumerExample.Repository {
    public interface IRepository {
        void SaveChanges();
        IQueryable<StudentiKafka> GetStudentiKafka();

        void InsertStudentiKafka(StudentiKafka studentiKafka);

    }
}
