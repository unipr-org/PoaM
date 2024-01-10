using System.ComponentModel.DataAnnotations.Schema;

namespace UniprExample.Repository.Model
{
    public class Corsi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Codice { get; set; }
        public required string Titolo { get; set; }
        public required string Docente { get; set; }
        public List<Esami> ListaEsami { get; set; } = new List<Esami>();
        public List<EsamiAlt> ListaEsamiAlt { get; set; } = new List<EsamiAlt>();
    }
}
