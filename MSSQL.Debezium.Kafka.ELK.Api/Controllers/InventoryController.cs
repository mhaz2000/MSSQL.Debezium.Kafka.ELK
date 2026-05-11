using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSSQL.Debezium.Kafka.ELK.Application.Commands.Inventories;

namespace MSSQL.Debezium.Kafka.ELK.Api.Controllers;

[ApiController]
[Route("api/Inventories")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Guid> Create(AddInventoryCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("Increase")]
    public async Task<Guid> Increase(IncreaseInventoryCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("Decrease")]
    public async Task<Guid> Decrease(DecreaseInventoryCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new RemoveProductInventoryCommand(id));
        return Ok();
    }
}
