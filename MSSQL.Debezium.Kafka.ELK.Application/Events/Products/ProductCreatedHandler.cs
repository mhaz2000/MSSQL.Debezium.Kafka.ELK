using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Events.Products;

namespace MSSQL.Debezium.Kafka.ELK.Application.Events.Products;

public class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product created: {notification.Name}");

        return Task.CompletedTask;
    }
}