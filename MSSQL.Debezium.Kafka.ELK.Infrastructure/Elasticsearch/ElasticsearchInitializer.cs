using Elastic.Clients.Elasticsearch;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Elasticsearch;

public static class ElasticsearchInitializer
{
    public static ElasticsearchClient CreateClient(string url)
    {
        var settings = new ElasticsearchClientSettings(new Uri(url))
            .DefaultFieldNameInferrer(field => field);

        return new ElasticsearchClient(settings);
    }
}