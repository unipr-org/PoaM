using Imu.ClientHttp.Abstraction;
using Imu.Shared;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Imu.ClientHttp
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
