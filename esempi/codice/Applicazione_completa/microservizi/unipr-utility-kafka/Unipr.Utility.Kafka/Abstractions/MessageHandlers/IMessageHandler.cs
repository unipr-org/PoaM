namespace Utility.Kafka.Abstractions.MessageHandlers;

public interface IMessageHandler {
    Task OnMessageReceivedAsync(string msg);
}
