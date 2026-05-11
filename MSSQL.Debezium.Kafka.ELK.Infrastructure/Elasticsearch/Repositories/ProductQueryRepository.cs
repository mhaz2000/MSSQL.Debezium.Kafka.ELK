using Elastic.Clients.Elasticsearch;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;
using MSSQL.Debezium.Kafka.ELK.Domain.QueryModels;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Elasticsearch.Repositories
{
    public class ProductQueryRepository(ElasticsearchClient elastic) : QueryRepository<ProductModel>(elastic, "products"), IProductQueryRepository;
}
