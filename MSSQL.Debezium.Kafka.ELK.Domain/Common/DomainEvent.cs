using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Domain.Common;

public abstract class DomainEvent : INotification
{
    public Guid AggregateId { get; set; }
    public abstract string AggregateType { get; }
    public abstract string Operation { get; }
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}