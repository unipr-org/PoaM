namespace Imu.Repository.Model
{
    public class CategoriaCatastale
    {
        public int Id { get; set; }
        public required string Codice { get; set; }
        public required string Descrizione { get; set; }

        public List<Immobile> Immobili { get; set; } = new List<Immobile>();
    }
}
