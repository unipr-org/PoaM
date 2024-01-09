using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniprExample.Repository.Model
{
    public class EsamiAlt
    {
        public int ID_CORSO { get; set; }
        public int ID_STUDENTE { get; set; }
        public int Voto { get; set; }
        public bool Lode { get; set; }
        public Corsi Corso { get; set; }
        public Studenti Studente { get; set; }
    }
}
