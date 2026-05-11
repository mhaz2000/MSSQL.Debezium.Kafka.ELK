using MSSQL.Debezium.Kafka.ELK.Domain.Common;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Events.Products;

public class ProductCreatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public DateTime CreatedAt { get; set; }

    public override string AggregateType => "products";
    public override string Operation => "index";


    public ProductCreatedEvent(Guid productId, string name, decimal price, DateTime createdAt)
    {
        AggregateId = productId;
        ProductId = productId;
        Name = name;
        Price = price; 
        CreatedAt = createdAt;
    }
}
