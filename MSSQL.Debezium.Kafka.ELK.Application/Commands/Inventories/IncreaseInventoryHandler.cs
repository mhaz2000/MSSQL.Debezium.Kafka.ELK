using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public class IncreaseInventoryHandler : IRequestHandler<IncreaseInventoryCommand, Guid>
{
    private readonly IProductInventoryCommandRepository _repository;

    public IncreaseInventoryHandler(IProductInventoryCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(IncreaseInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.FirstOrDefaultAsync(t => t.Id == request.InventoryId);
        if (inventory is null)
            throw new ArgumentNullException();

        inventory.Increase(request.Amount);
        await _repository.CommitAsync();

        return inventory.Id;
    }
}
