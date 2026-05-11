using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using MSSQL.Debezium.Kafka.ELK.Domain.Events.Products;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Entities;
public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; set; }
    private Product() { }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;

        AddDomainEvent(new ProductCreatedEvent(Id, name, price, CreatedAt));
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative");
        Price = newPrice;
    }
}
