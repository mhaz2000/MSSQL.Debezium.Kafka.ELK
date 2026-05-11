using MediatR;
using Microsoft.EntityFrameworkCore;
using MSSQL.Debezium.Kafka.ELK.Domain.Common;
using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using System.Text.Json;

namespace MSSQL.Debezium.Kafka.ELK.Infrastructure.Persistence;


public class ApplicationDbContext : DbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }


    public DbSet<Product> Products => Set<Product>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<ProductInventory> Inventories => Set<ProductInventory>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = domainEvent.OccurredOn,
                    Type = domainEvent.GetType().Name,
                    Content = JsonSerializer.Serialize(
                        domainEvent,
                        domainEvent.GetType()
                    )
                };

                outboxMessages.Add(outboxMessage);
            }

            entity.ClearDomainEvents();
        }

        await OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

}
