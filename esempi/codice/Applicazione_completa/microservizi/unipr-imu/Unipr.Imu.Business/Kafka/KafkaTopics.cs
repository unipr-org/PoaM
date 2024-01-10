using Microsoft.Extensions.DependencyInjection;

namespace Imu.Business.Kafka;

public class KafkaTopicsInput : AbstractKafkaTopics {

    public string Versamenti { get; set; } = "Versamenti";

    public override IEnumerable<string> GetTopics() => new List<string>() { Versamenti };

}
