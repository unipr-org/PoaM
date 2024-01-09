using Microsoft.Extensions.DependencyInjection;

namespace UniprExample.Business.Kafka;

public class KafkaTopicsOutput : AbstractKafkaTopics {
    public string Studenti { get; set; } = "Studenti";

    public override IEnumerable<string> GetTopics() => new List<string>() { Studenti };

}