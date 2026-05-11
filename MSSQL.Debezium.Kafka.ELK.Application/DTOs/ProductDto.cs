namespace MSSQL.Debezium.Kafka.ELK.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string CreatedAt
);