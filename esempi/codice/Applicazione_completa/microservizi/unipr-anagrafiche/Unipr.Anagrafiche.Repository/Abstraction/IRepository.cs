using Anagrafiche.Repository.Model;

namespace Anagrafiche.Repository.Abstraction
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CreateSoggetto(string nome, string cognome, string codiceFiscale, DateTime dataDiNascita, CancellationToken cancellationToken = default);
        Task<Soggetto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default);
    }
}
