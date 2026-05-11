using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Queries;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Stocks;

public class GetStocksQueryHandler
    : IRequestHandler<GetStocksQuery, PagedResult<StockDto>>
{
    private readonly IStockQueryRepository _repository;

    public GetStocksQueryHandler(IStockQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<StockDto>> Handle(
        GetStocksQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _repository.GetAllAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        return new PagedResult<StockDto>()
        {
            Items = data.data.Select(s => new StockDto(s.StockId, s.Name, s.Address, s.CreatedAt.ToLongDateString())).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            Total = data.count
        };
    }
}