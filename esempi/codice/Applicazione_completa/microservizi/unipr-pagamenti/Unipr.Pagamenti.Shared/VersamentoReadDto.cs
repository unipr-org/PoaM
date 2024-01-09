namespace Pagamenti.Shared
{
    public class VersamentoReadDto
    {
        public int Id { get; set; }
        public DateTime DataVersamento { get; set; }
        public DateTime DataContabile { get; set; }
        public decimal Importo { get; set; }
        public string? Informazioni { get; set; }
    }
}
