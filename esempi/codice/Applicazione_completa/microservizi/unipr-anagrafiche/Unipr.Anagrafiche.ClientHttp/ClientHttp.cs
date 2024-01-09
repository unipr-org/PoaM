using Anagrafiche.ClientHttp.Abstraction;
using Anagrafiche.Shared;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Anagrafiche.ClientHttp
{
    public class ClientHttp : IClientHttp
    {
        private readonly HttpClient _httpClient;
        public ClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> CreateSoggetto(SoggettoDto soggetto, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsync($"/Soggetto/CreateSoggetto", JsonContent.Create(soggetto), cancellationToken);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
        }

        public async Task<SoggettoDto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default)
        {
            var queryString = QueryString.Create(new Dictionary<string, string?>() {
                { "idAnagrafica", idAnagrafica.ToString(CultureInfo.InvariantCulture) }
            });
            var response = await _httpClient.GetAsync($"/Soggetto/ReadSoggetto{queryString}", cancellationToken);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<SoggettoDto?>(cancellationToken: cancellationToken);
        }

        public async Task<SoggettoDto?> GetFromDemografico(string codiceFiscale, CancellationToken cancellationToken = default)
        {
            var queryString = QueryString.Create(new Dictionary<string, string?>() {
                { "codiceFiscale", codiceFiscale.ToString(CultureInfo.InvariantCulture) }
            });
            var response = await _httpClient.GetAsync($"/Soggetto/GetFromDemografico{queryString}", cancellationToken);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<SoggettoDto>(cancellationToken: cancellationToken);
        }

    }
}
