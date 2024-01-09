namespace UniprExample.Shared.Dto
{
    public class StudenteDto
    {
        public int Matricola { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public DateTime DataDiNascita { get; set; }
    }

    public class StudenteInsertDto {
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public DateTime DataDiNascita { get; set; }
    }
}
