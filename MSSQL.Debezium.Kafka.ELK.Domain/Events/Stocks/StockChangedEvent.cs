using MSSQL.Debezium.Kafka.ELK.Domain.Common;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Events.Stocks;

public class StockChangedEvent : DomainEvent
{
    public Guid StockId { get; }
    public string Name { get; }
    public string Address { get; }
    public DateTime CreatedAt { get; set; }

    public override string AggregateType => "stocks";
    public override string Operation => "index";


    public StockChangedEvent(Guid stockId, string name, string address, DateTime createdAt)
    {
        AggregateId = stockId;
        StockId = stockId;
        Name = name;
        Address = address;
        CreatedAt = createdAt;
    }
}

