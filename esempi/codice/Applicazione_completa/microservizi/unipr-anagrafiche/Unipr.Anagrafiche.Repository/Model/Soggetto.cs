using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagrafiche.Repository.Model
{
    public class Soggetto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string CodiceFiscale { get; set; }
        public DateTime DataDiNascita { get; set; }

        public List<Recapito> ListaRecapiti { get; set; } = new List<Recapito>();
    }
}
