using MSSQL.Debezium.Kafka.ELK.Domain.Common;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Events.Inventories;

public class InventoryRemovedEvent : DomainEvent
{
    public override string AggregateType => "Inventories";
    public override string Operation => "delete";

    public InventoryRemovedEvent(Guid id)
    {
        AggregateId = id;
    }
}