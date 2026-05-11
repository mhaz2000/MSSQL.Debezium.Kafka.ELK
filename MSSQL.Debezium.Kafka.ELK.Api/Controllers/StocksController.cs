using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSSQL.Debezium.Kafka.ELK.Application.Commands.Stocks;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;
using MSSQL.Debezium.Kafka.ELK.Application.Queries.Stocks;

namespace MSSQL.Debezium.Kafka.ELK.Api.Controllers;

[ApiController]
[Route("api/stocks")]
public class StocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public StocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Guid> Create(AddStockCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut]
    public async Task<Guid> Update(UpdateStockCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpGet]
    public async Task<PagedResult<StockDto>> GetList(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        return await _mediator.Send(new GetStocksQuery(page, pageSize));
    }
}
