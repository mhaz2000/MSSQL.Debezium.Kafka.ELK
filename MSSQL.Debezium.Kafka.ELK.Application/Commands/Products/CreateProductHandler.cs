using MediatR;
using MSSQL.Debezium.Kafka.ELK.Domain.Entities;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Commands.Products;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductCommandRepository _repository;

    public CreateProductHandler(IProductCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Price);

        await _repository.AddAsync(product);
        await _repository.CommitAsync();
        return product.Id;
    }
}