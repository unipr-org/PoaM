using System.ComponentModel.DataAnnotations;

namespace ConsumerExample.Repository.Model {
    public class StudentiKafka {
        [Key]
        public int Matricola { get; set; }
        public string Cognome { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime DataDiNascita { get; set; }
    }
}
