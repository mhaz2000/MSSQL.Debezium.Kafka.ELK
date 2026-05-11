using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public record DecreaseInventoryCommand(
    Guid InventoryId,
    int Amount
) : IRequest<Guid>;
