using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Products;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductQueryRepository _repository;

    public GetProductsQueryHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _repository.GetAllAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        return new PagedResult<ProductDto>()
        {
            Items = data.data.Select(s => new ProductDto(s.ProductId, s.Name, s.Price, s.CreatedAt.ToLongDateString())).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            Total = data.count
        };
    }
}