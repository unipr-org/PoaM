using Anagrafiche.Shared;
using Imu.Business.Abstraction;
using Imu.Repository.Abstraction;
using Imu.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Json;

namespace Imu.Business
{
    public class Business : IBusiness
    {
        private readonly IRepository _repository;
        private readonly Anagrafiche.ClientHttp.Abstraction.IClientHttp _clientHttp;
        private readonly ILogger<Business> _logger;
        public Business(IRepository repository, Anagrafiche.ClientHttp.Abstraction.IClientHttp clientHttp, ILogger<Business> logger)
        {
            _repository = repository;
            _clientHttp = clientHttp;
            _logger = logger;
        }

        public async Task AssociaAnagraficaImmobile(AssociaAnagraficaImmobileDto associaAnagraficaImmobileDto, CancellationToken cancellationToken = default)
        {
            SoggettoDto? soggetto = await _clientHttp.ReadSoggetto(associaAnagraficaImmobileDto.IdAnagrafica, cancellationToken);

            if (soggetto is null)
                throw new Exception($"soggetto id {associaAnagraficaImmobileDto.IdAnagrafica} non trovato");

            await _repository.AssociaAnagraficaImmobile(associaAnagraficaImmobileDto, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateCategoriaCatastale(CategoriaCatastaleInsertDto categoriaCatastaleInsertDto, CancellationToken cancellationToken = default)
        {
            await _repository.CreateCategoriaCatastale(categoriaCatastaleInsertDto, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateImmobile(ImmobileInsertDto immobileInsertDto, CancellationToken cancellationToken = default)
        {
            await _repository.CreateImmobile(immobileInsertDto, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
