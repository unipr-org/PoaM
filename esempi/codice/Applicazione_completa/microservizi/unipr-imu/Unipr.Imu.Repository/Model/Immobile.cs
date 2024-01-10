namespace Imu.Repository.Model
{
    public class Immobile
    {
        public int Id { get; set; }
        public int IdCategoriaCatastale { get; set; }
        public decimal Superficie { get; set; }
        public decimal Rendita { get; set; }
        public string? Sezione { get; set; }
        public required string Foglio { get; set; }
        public required string Particella { get; set; }
        public string? Subalterno { get; set; }
        public required string Indirizzo { get; set; }
        public required string NumeroCivico { get; set; }
        public required string Cap { get; set; }
        public required string Provincia { get; set; }
        public required string Localita { get; set; }

        public CategoriaCatastale? CategoriaCatastale { get; set; }

        public List<AnagraficaImmobile> AnagraficheImmobile { get; set; } = new List<AnagraficaImmobile>();
    }
}
