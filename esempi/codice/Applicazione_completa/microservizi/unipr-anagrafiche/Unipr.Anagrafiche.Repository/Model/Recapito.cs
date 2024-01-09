using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagrafiche.Repository.Model
{
    public class Recapito
    {
        public int Id { get; set; }
        public int IdSoggetto { get; set; }
        public string TipoIndirizzo { get; set; }
        public string Indirizzo { get; set; }
        public string NumeroCivico { get; set; }
        public string Cap { get; set; }
        public string Provincia { get; set; }
        public string Localita { get; set; }

        public Soggetto Soggetto { get; set; }


    }
}
