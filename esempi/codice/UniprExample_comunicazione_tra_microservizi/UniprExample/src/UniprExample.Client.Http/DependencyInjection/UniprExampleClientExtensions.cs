using Microsoft.Extensions.Configuration;
using UniprExample.Client.Http;
using UniprExample.Client.Http.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class UniprExampleClientExtensions {

    public static IServiceCollection AddTransactionRepClient(this IServiceCollection services, IConfiguration configuration) {

        IConfigurationSection confSection = configuration.GetSection(UniprExampleClientOptions.SectionName);
        UniprExampleClientOptions options = confSection.Get<UniprExampleClientOptions>() ?? new();

        services.AddHttpClient<IUniprExampleClient, UniprExampleClient>(o => {          
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

        return services;
    }

}
