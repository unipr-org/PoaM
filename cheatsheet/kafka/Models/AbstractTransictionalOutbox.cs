namespace Utility.Kafka.Models {
    public abstract class AbstractTransactionalOutbox {
        public long Id { get; set; }
        public string Tabella { get; set; } = string.Empty;
        public string Messaggio { get; set; } = string.Empty;
    }
}
