namespace MSSQL.Debezium.Kafka.ELK.Domain.QueryModels;
public class ProductModel
{
    public string Name {get; set;}
    public decimal Price {get; set;}
    public DateTime CreatedAt {get; set;}
    public Guid ProductId { get; set; }
}

