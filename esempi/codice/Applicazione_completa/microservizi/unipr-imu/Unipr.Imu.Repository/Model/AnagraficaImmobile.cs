namespace Imu.Repository.Model
{
    public class AnagraficaImmobile
    {
        public int Id { get; set; }
        public int IdAnagrafica { get; set; }
        public int IdImmobile { get; set; }
        public decimal Quota { get; set; }

        public Immobile? Immobile { get; set; }
    }
}
