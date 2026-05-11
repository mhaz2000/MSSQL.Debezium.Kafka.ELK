using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using MSSQL.Debezium.Kafka.ELK.Domain.Events.Inventories;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Entities;

public class ProductInventory : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Guid StockId { get; private set; }
    public int Quantity { get; private set; }

    private ProductInventory() { }

    public ProductInventory(Guid productId, Guid stockId, int quantity)
    {
        ProductId = productId;
        StockId = stockId;
        Quantity = quantity;

        AddDomainEvent(new InventoryChangedEvent(Id,productId, stockId, quantity));

    }

    public void Increase(int amount)
    {
        Quantity += amount;
        AddDomainEvent(new InventoryChangedEvent(Id, ProductId, StockId, Quantity));
    }

    public void Decrease(int amount)
    {
        if (Quantity < amount)
            throw new InvalidOperationException("Insufficient stock");
        Quantity -= amount;

        AddDomainEvent(new InventoryChangedEvent(Id, ProductId, StockId, Quantity));

    }

    public void Delete()
    {
        AddDomainEvent(new InventoryRemovedEvent(Id));
    }
}
