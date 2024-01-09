using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Utility.Kafka.Abstractions.Clients;
using Utility.Kafka.Abstractions.MessageHandlers;
using Utility.Kafka.Clients;
using Utility.Kafka.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class KafkaExtensions {

    /// <summary>
    /// Registra il <see cref="ConsumerService{TKafkaTopicsInput}"/> e il <typeparamref name="TProducerService"/> di tipo <see cref="ProducerService{TKafkaTopicsOutput}"/>
    /// </summary>
    /// <typeparam name="TKafkaTopicsInput"></typeparam>
    /// <typeparam name="TKafkaTopicsOutput"></typeparam>
    /// <typeparam name="TMessageHandlerFactory"></typeparam>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaServices<TKafkaTopicsInput, TKafkaTopicsOutput, TMessageHandlerFactory, TProducerService>(
        this IServiceCollection services, IConfiguration configuration)
        where TKafkaTopicsInput : class, IKafkaTopics
        where TKafkaTopicsOutput : class, IKafkaTopics
        where TMessageHandlerFactory : class, IMessageHandlerFactory
        where TProducerService : ProducerService<TKafkaTopicsOutput> {

        if (!IsEnable(configuration))
            return services;

        services.AddAdministatorClient(configuration);

        services.AddConsumerService<TKafkaTopicsInput, TMessageHandlerFactory>(configuration);

        services.AddProducerService<TKafkaTopicsOutput, TProducerService>(configuration);

        return services;
    }

    /// <summary>
    /// Registra il <see cref="ConsumerService{TKafkaTopicsInput}"/>
    /// </summary>
    /// <typeparam name="TKafkaTopicsInput"></typeparam>
    /// <typeparam name="TMessageHandlerFactory"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaConsumerService<TKafkaTopicsInput, TMessageHandlerFactory>(
        this IServiceCollection services, IConfiguration configuration)
        where TKafkaTopicsInput : class, IKafkaTopics
        where TMessageHandlerFactory : class, IMessageHandlerFactory {

        if (!IsEnable(configuration))
            return services;

        services.AddAdministatorClient(configuration);

        services.AddConsumerService<TKafkaTopicsInput, TMessageHandlerFactory>(configuration);

        return services;
    }

    /// <summary>
    /// Registra il <typeparamref name="TProducerService"/> di tipo <see cref="ProducerService{TKafkaTopicsOutput}"/>
    /// </summary>
    /// <typeparam name="TKafkaTopicsOutput"></typeparam>
    /// <typeparam name="TProducerService"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddKafkaProducerService<TKafkaTopicsOutput, TProducerService>(
        this IServiceCollection services, IConfiguration configuration)
        where TKafkaTopicsOutput : class, IKafkaTopics
        where TProducerService : ProducerService<TKafkaTopicsOutput> {

        if (!IsEnable(configuration))
            return services;

        services.AddAdministatorClient(configuration);

        services.AddProducerService<TKafkaTopicsOutput, TProducerService>(configuration);

        return services;
    }

    private static bool IsEnable(IConfiguration configuration) {
        KafkaOptions options = configuration.GetSection(KafkaOptions.SectionName).Get<KafkaOptions>() ?? new KafkaOptions();

        return options.Enable;
    }

    private static IServiceCollection AddAdministatorClient(this IServiceCollection services, IConfiguration configuration) {
        // KafkaAdminClientOptions
        services.Configure<KafkaAdminClientOptions>(
            configuration.GetSection(KafkaAdminClientOptions.SectionName));
        // AdministatorClient
        services.AddSingleton<IAdministatorClient, AdministatorClient>();

        return services;
    }

    private static IServiceCollection AddConsumerService<TKafkaTopicsInput, TMessageHandlerFactory>(
    this IServiceCollection services, IConfiguration configuration)
    where TKafkaTopicsInput : class, IKafkaTopics
    where TMessageHandlerFactory : class, IMessageHandlerFactory {

        // Background ConsumerService
        services.AddSingleton<IHostedService, ConsumerService<TKafkaTopicsInput>>();

        // MessageHandlerFactory
        services.AddSingleton<IMessageHandlerFactory, TMessageHandlerFactory>();

        // KafkaConsumerClientOptions
        services.Configure<KafkaConsumerClientOptions>(configuration.GetSection(KafkaConsumerClientOptions.SectionName));

        // ConsumerClient
        services.AddSingleton<IConsumerClient, ConsumerClient>();

        // KafkaTopicsInput
        services.Configure<TKafkaTopicsInput>(
            configuration.GetSection(AbstractKafkaTopics.SectionName));

        return services;
    }

    private static IServiceCollection AddProducerService<TKafkaTopicsOutput, TProducerService>(
    this IServiceCollection services, IConfiguration configuration)
    where TKafkaTopicsOutput : class, IKafkaTopics
    where TProducerService : ProducerService<TKafkaTopicsOutput> {

        // KafkaProducerServiceOptions
        services.Configure<KafkaProducerServiceOptions>(
            configuration.GetSection(KafkaProducerServiceOptions.SectionName));

        // Background ProducerService
        services.AddSingleton<IHostedService, TProducerService>();

        // KafkaProducerClientOptions
        services.Configure<KafkaProducerClientOptions>(
            configuration.GetSection(KafkaProducerClientOptions.SectionName));
        // ProducerClient
        services.AddSingleton<IProducerClient, ProducerClient>();

        // KafkaTopicsOutput
        services.Configure<TKafkaTopicsOutput>(
            configuration.GetSection(AbstractKafkaTopics.SectionName));

        return services;
    }
}
