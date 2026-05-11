using MSSQL.Debezium.Kafka.ELK.Domain.Entities;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

public interface IProductInventoryCommandRepository : ICommandRepository<ProductInventory>;