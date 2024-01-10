namespace Utility.Kafka.Abstractions.MessageHandlers;

public interface IMessageHandlerFactory {
    IMessageHandler Create(string topic, IServiceProvider serviceProvider);
}
