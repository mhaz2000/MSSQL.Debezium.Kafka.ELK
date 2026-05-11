using MediatR;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Stocks;
public record AddStockCommand(string Name, string Address) : IRequest<Guid>;
