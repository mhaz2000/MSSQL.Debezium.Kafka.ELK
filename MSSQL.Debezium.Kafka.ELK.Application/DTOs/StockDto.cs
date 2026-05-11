namespace MSSQL.Debezium.Kafka.ELK.Application.DTOs;
public record StockDto(
        Guid Id,
    string Name,
    string Address,
    string CreatedAt
);
