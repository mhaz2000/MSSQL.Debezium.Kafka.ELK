using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence.Repositories;

public class ProductInventoryCommandRepository(ApplicationDbContext context) : CommandRepository<ProductInventory>(context), IProductInventoryCommandRepository;
