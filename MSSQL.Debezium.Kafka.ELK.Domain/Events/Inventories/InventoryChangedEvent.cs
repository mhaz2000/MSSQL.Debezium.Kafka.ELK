using MSSQL.Debezium.Kafka.ELK.Domain.Common;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Events.Inventories;

public class InventoryChangedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public Guid StockId { get; }
    public int Quantity { get; }

    public override string AggregateType => "Inventories";
    public override string Operation => "index";

    public InventoryChangedEvent(Guid id, Guid productId, Guid stockId, int quantity)
    {
        AggregateId = id;
        ProductId = productId;
        StockId = stockId;
        Quantity = quantity;
    }
}
