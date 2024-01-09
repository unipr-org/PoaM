namespace UniprExample.Repository.Model
{
    public class Esami
    {
        public int CorsiId { get; set; }
        public int StudentiId { get; set; }

        public int Voto { get; set; }
        public bool Lode { get; set; }
        public Corsi Corso { get; set; }
        public Studenti Studente { get; set; }
    }
}
