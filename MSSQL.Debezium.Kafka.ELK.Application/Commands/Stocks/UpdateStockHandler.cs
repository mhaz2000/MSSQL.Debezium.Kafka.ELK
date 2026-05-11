using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Stocks;

public class UpdateStockHandler(IStockCommandRepository repository) : IRequestHandler<UpdateStockCommand, Guid>
{
    public async Task<Guid> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await repository.FirstOrDefaultAsync(t => t.Id == request.Id);
        if (stock is null)
            throw new ArgumentNullException();

        stock.Update(request.Name, request.Address);
        await repository.CommitAsync();

        return stock.Id;
    }
}
