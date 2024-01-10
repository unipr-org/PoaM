using System.ComponentModel.DataAnnotations.Schema;

namespace UniprExample.Repository.Model
{
    public record Studenti
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Matricola { get; set; }
        public required string Cognome { get; set; }
        public required string Nome { get; set; }
        public DateTime DataDiNascita { get; set; }
        public List<Esami> ListaEsami { get; set; } = new List<Esami>();
        public List<EsamiAlt> ListaEsamiAlt { get; set; } = new List<EsamiAlt>();
    }
}
