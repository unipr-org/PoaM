namespace UniprExample.Shared.Dto
{
    public class EsameStudenteDto
    {
        public int Matricola { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public string NomeCorso { get; set; } = string.Empty;
        public int Voto { get; set; }
        public bool Lode { get; set; }

    }
}
