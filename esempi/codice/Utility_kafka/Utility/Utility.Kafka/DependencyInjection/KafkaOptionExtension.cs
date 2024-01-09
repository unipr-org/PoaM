namespace Microsoft.Extensions.DependencyInjection;

public class KafkaOptions {
    public const string SectionName = "Kafka";

    /// <summary>
    /// Flag che indica se abilitare i servizi Kafka
    /// </summary>
    public bool Enable { get; set; } = true;
}

public abstract class KafkaClientOptions {
    public string BootstrapServers { get; set; } = string.Empty;

}

public class KafkaAdminClientOptions : KafkaClientOptions {
    public const string SectionName = "Kafka:AdminClient";
}

public class KafkaProducerClientOptions : KafkaClientOptions {
    public const string SectionName = "Kafka:ProducerClient";
}

public class KafkaConsumerClientOptions : KafkaClientOptions {
    public const string SectionName = "Kafka:ConsumerClient";

    public string GroupId { get; set; } = string.Empty;
}

public class KafkaProducerServiceOptions {
    public const string SectionName = "Kafka:ProducerService";

    /// <summary>
    /// Secondi di attesa per la prima chiamata metodo ExecuteTask
    /// </summary>
    public int DelaySeconds { get; set; } = 60;

    /// <summary>
    /// Secondi di attesa tra le chiamate al metodo ExecuteTask
    /// </summary>
    public int IntervalSeconds { get; set; } = 60;
}

public interface IKafkaTopics {
    IEnumerable<string> GetTopics();
}

public abstract class AbstractKafkaTopics : IKafkaTopics {
    public const string SectionName = "Kafka:Topics";

    public abstract IEnumerable<string> GetTopics();
}