using UniprExample.Shared.Dto;

namespace UniprExample.Business
{
    public interface IBusiness
    {
        List<StudenteDto> GetStudenti();
        List<StudenteDto> GetStudenti(string cognome, string nome);
        StudenteDto? GetStudenteByKey(int matricola);
        IEnumerable<EsameStudenteDto> GetEsamiStudente(int corsoId);
        StudenteDto InsertStudente(StudenteInsertDto insertDto);
    }
}
