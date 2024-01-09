using Microsoft.Extensions.DependencyInjection;

namespace Pagamenti.Business.Kafka;

public class KafkaTopicsOutput : AbstractKafkaTopics {
    public string Versamenti { get; set; } = "Versamenti";

    public override IEnumerable<string> GetTopics() => new List<string>() { Versamenti };

}