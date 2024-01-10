using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UniprExample.Client.Http.Abstractions;
using UniprExample.Shared.Dto;

namespace UniprExample.Client.Http;

public class UniprExampleClient : IUniprExampleClient {

    readonly HttpClient _httpClient;

    public UniprExampleClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<StudenteDto?> GetStudenteByKeyAsync(int matricola, CancellationToken cancellationToken = default) {

        QueryString queryString = QueryString.Create(new Dictionary<string, string?>() {
                { "matricola", matricola.ToString(CultureInfo.InvariantCulture) }
            });

        HttpResponseMessage response = await _httpClient.GetAsync($"/webapi/UniprExample/GetStudenteByKey{queryString}", cancellationToken);

        if (response.IsSuccessStatusCode) {
            return await response.Content.ReadFromJsonAsync<StudenteDto>((JsonSerializerOptions?)null, cancellationToken);
        } else if (response.StatusCode == HttpStatusCode.NotFound) {
            return null;
        } else {
            throw new Exception($"La chiamata GetStudenteByKey per matricola {matricola} ha ritornato il seguente StatusCode: {response.StatusCode}");
        }
    }

}