using Microsoft.Extensions.DependencyInjection;

namespace ConsumerExample.Business.Kafka;

public class KafkaTopicsInput : AbstractKafkaTopics {

    public string Studenti { get; set; } = "Studenti";

    public override IEnumerable<string> GetTopics() => new List<string>() { Studenti };

}
