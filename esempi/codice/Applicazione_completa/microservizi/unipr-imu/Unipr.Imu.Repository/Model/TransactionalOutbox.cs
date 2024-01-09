namespace Imu.Repository.Model
{
    public class TransactionalOutbox {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Nome della Tabella modificata
        /// </summary>
        public string Tabella { get; set; } = string.Empty;

        /// <summary>
        /// Messaggio JSON di tipo OperationMessage contenente il record inserito/modificato/eliminato
        /// </summary>
        public string Messaggio { get; set; } = string.Empty;
    }
}
