namespace MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];
    public long Total { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}