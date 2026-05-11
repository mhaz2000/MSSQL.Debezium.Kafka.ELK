using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Stocks;

public class AddStockHandler(IStockCommandRepository repository) : IRequestHandler<AddStockCommand, Guid>
{
    public async Task<Guid> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        var stock = new Stock(request.Name, request.Address);

        await  repository.AddAsync(stock);
        await repository.CommitAsync();

        return stock.Id;
    }
}
