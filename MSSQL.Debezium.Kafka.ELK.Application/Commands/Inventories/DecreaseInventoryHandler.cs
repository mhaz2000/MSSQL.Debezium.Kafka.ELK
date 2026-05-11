using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public class DecreaseInventoryHandler : IRequestHandler<DecreaseInventoryCommand, Guid>
{
    private readonly IProductInventoryCommandRepository _repository;

    public DecreaseInventoryHandler(IProductInventoryCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DecreaseInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.FirstOrDefaultAsync(t => t.Id == request.InventoryId);
        if (inventory is null)
            throw new ArgumentNullException();

        inventory.Decrease(request.Amount);
        await _repository.CommitAsync();

        return inventory.Id;
    }
}

public class RemoveProductInventoryHandler : IRequestHandler<RemoveProductInventoryCommand>
{
    private readonly IProductInventoryCommandRepository _repository;

    public RemoveProductInventoryHandler(IProductInventoryCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveProductInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _repository.FirstOrDefaultAsync(t => t.Id == request.InventoryId);
        if (inventory is null)
            throw new ArgumentNullException();

        inventory.Delete();
        await _repository.DeleteAsync(inventory.Id);
        await _repository.CommitAsync();
    }
}
