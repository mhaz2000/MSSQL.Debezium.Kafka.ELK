using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Stocks;

public record UpdateStockCommand(Guid Id, string Name, string Address) : IRequest<Guid>;
