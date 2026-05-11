using Microsoft.EntityFrameworkCore;
using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;
using System.Linq.Expressions;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence.Repositories;
public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;

    public CommandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _context.Set<T>().AddAsync(entity);
    }

    public Task CommitAsync()
        => _context.SaveChangesAsync();

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");

        _context.Set<T>().Remove(entity);

    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Update(entity);
    }
}
