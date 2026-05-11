using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public record RemoveProductInventoryCommand(
    Guid InventoryId
) : IRequest;