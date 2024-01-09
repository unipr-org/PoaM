namespace Utility.Kafka.Exceptions;

public class MessageHandlerException : ArgumentException {
    /// <inheritdoc/>
    public MessageHandlerException() : base() { }

    /// <inheritdoc/>
    public MessageHandlerException(string? message) : base(message) { }

    /// <inheritdoc/>
    public MessageHandlerException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <inheritdoc/>
    public MessageHandlerException(string? message, string? paramName) : base(message, paramName) { }

    /// <inheritdoc/>
    public MessageHandlerException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException) { }
}

