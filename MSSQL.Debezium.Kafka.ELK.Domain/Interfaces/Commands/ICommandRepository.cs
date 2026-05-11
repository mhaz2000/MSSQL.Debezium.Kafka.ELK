using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using System.Linq.Expressions;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

public interface ICommandRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task UpdateAsync(T entity);
    Task CommitAsync();
}
