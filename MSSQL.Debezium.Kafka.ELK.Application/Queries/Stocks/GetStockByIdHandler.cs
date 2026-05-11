using MediatR;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Domain.Interfaces.Commands;

namespace MSSQL.Debezium.Kafka.ELK.Application.Queries.Stocks;

public class GetStockByIdHandler : IRequestHandler<GetStockByIdQuery, StockDto?>
{
    private readonly IStockCommandRepository _repository;

    public GetStockByIdHandler(IStockCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<StockDto?> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await _repository.FirstOrDefaultAsync(t => t.Id == request.Id);

        if (stock == null)
            return null;

        return new StockDto(stock.Id, stock.Name, stock.Address, stock.CreatedAt.ToShortDateString());
    }
}