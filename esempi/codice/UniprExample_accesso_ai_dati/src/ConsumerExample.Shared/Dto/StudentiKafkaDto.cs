namespace ConsumerExample.Shared.Dto;

public class StudentiKafkaDto {
    public int Matricola { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public DateTime DataDiNascita { get; set; }
}
