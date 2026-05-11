using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Products;

public record CreateProductCommand(
    string Name,
    decimal Price
) : IRequest<Guid>;