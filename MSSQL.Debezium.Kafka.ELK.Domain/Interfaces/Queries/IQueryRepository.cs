using MSSQL.Debezium.Kafka.ELK.Domain.Common;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;

public interface IQueryRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<T> data, long count)> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

