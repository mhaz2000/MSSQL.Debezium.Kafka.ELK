using MSSQL.Debezium.Kafka.ELK.Domain.QueryModels;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries
{
    public interface IProductQueryRepository : IQueryRepository<ProductModel>;
}
