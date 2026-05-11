using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSSQL.Debezium.Kafka.ELK.Application.Commands.Products;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs;
using MSSQL.Debezium.Kafka.ELK.Application.DTOs.Base;
using MSSQL.Debezium.Kafka.ELK.Application.Queries.Products;

namespace MSSQL.Debezium.Kafka.ELK.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Guid> Create(CreateProductCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpGet("{id}")]
    public async Task<ProductDto?> Get(Guid id)
    {
        return await _mediator.Send(new GetProductByIdQuery(id));
    }

    [HttpGet]
    public async Task<PagedResult<ProductDto>> GetList(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        return await _mediator.Send(new GetProductsQuery(page, pageSize));
    }

}
