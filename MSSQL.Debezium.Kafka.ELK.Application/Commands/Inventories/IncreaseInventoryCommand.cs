using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public record IncreaseInventoryCommand(
    Guid InventoryId,
    int Amount
) : IRequest<Guid>;
