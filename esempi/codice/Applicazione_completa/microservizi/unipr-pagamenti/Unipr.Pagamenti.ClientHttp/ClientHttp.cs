using Pagamenti.ClientHttp.Abstraction;

namespace Pagamenti.ClientHttp
{
    public class ClientHttp : IClientHttp
    {
        private readonly HttpClient _httpClient;
        public ClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
