using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

public class AddInventoryHandler : IRequestHandler<AddInventoryCommand, Guid>
{
    private readonly IProductInventoryCommandRepository _repository;

    public AddInventoryHandler(IProductInventoryCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventory = new ProductInventory(
            request.ProductId,
            request.StockId,
            request.Quantity
        );

        await _repository.AddAsync(inventory);
        await _repository.CommitAsync();

        return inventory.Id;
    }
}
