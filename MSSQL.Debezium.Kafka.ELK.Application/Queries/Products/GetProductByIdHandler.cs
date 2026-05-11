using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Products;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductCommandRepository _repository;

    public GetProductByIdHandler(IProductCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.FirstOrDefaultAsync(t=> t.Id == request.Id);

        if (product == null)
            return null;

        return new ProductDto(product.Id, product.Name, product.Price, product.CreatedAt.ToShortDateString());
    }
}