using Anagrafiche.Shared;

namespace Anagrafiche.Business.Abstraction
{
    public interface IBusiness
    {

        Task CreateSoggetto(SoggettoDto soggetto, CancellationToken cancellationToken = default);
        Task<SoggettoDto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default);
        Task<SoggettoDto> GetFromDemografico(string codiceFiscale, CancellationToken cancellationToken = default);
    }
}
