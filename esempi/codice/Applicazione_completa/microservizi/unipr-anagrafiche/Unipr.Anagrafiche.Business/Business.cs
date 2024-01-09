using Anagrafiche.Business.Abstraction;
using Anagrafiche.Repository.Abstraction;
using Anagrafiche.Repository.Model;
using Anagrafiche.Shared;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace Anagrafiche.Business
{
    public class Business : IBusiness
    {
        private readonly IRepository _repository;
        private readonly ILogger<Business> _logger;
        public Business(IRepository repository, ILogger<Business> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateSoggetto(SoggettoDto soggetto, CancellationToken cancellationToken = default)
        {
            await _repository.CreateSoggetto(soggetto.Nome, soggetto.Cognome, soggetto.CodiceFiscale, soggetto.DataDiNascita, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<SoggettoDto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default)
        {
            var soggetto = await _repository.ReadSoggetto(idAnagrafica, cancellationToken);

            if (soggetto is null)
                return null;

            return new SoggettoDto
            {
                Nome = soggetto.Nome,
                Cognome = soggetto.Cognome,
                CodiceFiscale = soggetto.CodiceFiscale,
                DataDiNascita = soggetto.DataDiNascita,
            };
        }

        public async Task<SoggettoDto> GetFromDemografico(string codiceFiscale, CancellationToken cancellationToken = default)
        {
            // qui chiamaremo il nostro demografico
            // demograficoClient.GetSoggetto(codiceFiscale, cancellationToken);
            return await Task.FromResult<SoggettoDto>(new SoggettoDto());
        }
    }
}
