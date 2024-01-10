using Anagrafiche.Shared;

namespace Anagrafiche.ClientHttp.Abstraction
{
    public interface IClientHttp
    {
        Task<string?> CreateSoggetto(SoggettoDto soggetto, CancellationToken cancellationToken = default);
        Task<SoggettoDto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default);
        Task<SoggettoDto?> GetFromDemografico(string codiceFiscale, CancellationToken cancellationToken = default);
    }
}
