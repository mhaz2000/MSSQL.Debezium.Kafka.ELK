using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Events.Inventories;

namespace MSSQL.Debezium.Kafka.ELK.Application.Events.Inventories;

public class InventoryChangedHandler : INotificationHandler<InventoryChangedEvent>
{
    public Task Handle(InventoryChangedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Stock Quantity changed to: {notification.Quantity}");

        return Task.CompletedTask;
    }
}

