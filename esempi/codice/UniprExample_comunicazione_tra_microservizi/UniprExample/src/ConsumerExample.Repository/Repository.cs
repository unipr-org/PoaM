using ConsumerExample.Repository.Model;

namespace ConsumerExample.Repository {
    public class Repository : IRepository {
        private readonly ConsumerExampleDbContext _consumerExampleDbContext;
        public Repository(ConsumerExampleDbContext consumerExampleDbContext) {
            _consumerExampleDbContext = consumerExampleDbContext;
        }

        public void SaveChanges() => _consumerExampleDbContext.SaveChanges();

        public IQueryable<StudentiKafka> GetStudentiKafka() {
            return _consumerExampleDbContext.StudentiKafkaList;
        }

        public void InsertStudentiKafka(StudentiKafka studentiKafka) {
            _consumerExampleDbContext.StudentiKafkaList.Add(studentiKafka);
        }

    }
}
