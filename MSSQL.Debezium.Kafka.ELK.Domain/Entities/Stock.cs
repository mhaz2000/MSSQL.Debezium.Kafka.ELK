using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using MSSQL.Debezium.Kafka.ELK.Domain.Events.Stocks;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Entities;

public class Stock : BaseEntity
{
    public string Name { get; private set; }
    public string Address { get; private set; }

    private Stock() { }

    public Stock(string name, string address)
    {
        Name = name;
        Address = address;

        AddDomainEvent(new StockChangedEvent(Id, name, address, CreatedAt));
    }

    public void Update(string name, string address)
    {
        Name = name;
        Address = address;

        AddDomainEvent(new StockChangedEvent(Id, name, address, CreatedAt));
    }
}