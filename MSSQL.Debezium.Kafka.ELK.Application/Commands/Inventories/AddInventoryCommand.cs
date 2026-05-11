using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public record AddInventoryCommand(
    Guid ProductId,
    Guid StockId,
    int Quantity
) : IRequest<Guid>;
