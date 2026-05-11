using Confluent.Kafka;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Kafka;

public class KafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string key, string value)
    {
        await _producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = value });
    }
}