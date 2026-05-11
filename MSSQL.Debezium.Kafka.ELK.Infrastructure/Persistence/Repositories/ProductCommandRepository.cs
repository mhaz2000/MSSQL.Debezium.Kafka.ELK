using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence.Repositories;

public class ProductCommandRepository(ApplicationDbContext context) : CommandRepository<Product>(context), IProductCommandRepository;
