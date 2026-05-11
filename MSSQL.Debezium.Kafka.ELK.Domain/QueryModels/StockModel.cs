namespace MSSQL.Debezium.Kafka.ELK.Domain.QueryModels;

public class StockModel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid StockId { get; set; }
}