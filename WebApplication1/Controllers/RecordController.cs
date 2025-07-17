using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Commands.Records.Create;
using WebApplication1.Commands.Records.Delete;
using WebApplication1.Commands.Records.Put;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Queries.Record;
using WebApplication1.Queries.Record.GetAllById;

namespace WebApplication1.Controllers;


[Route("api/record")]
[ApiController]
public class RecordController : ControllerBase
{
    private readonly IMediator _mediator; 

    public RecordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new GetRecordListQuery()));
    }

    [HttpGet("{id}") , Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _mediator.Send(new GetRecordListById(id)));
    }
    

    [HttpPost , Authorize]
    [ProducesResponseType(typeof(Record) , StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(CreateRecordCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    
    [HttpPut("{id}") , Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] InputRecordDto? record)
    {
        return Ok(await _mediator.Send(new PutRecordCommand(id , record.RecordText)));
    }

    [HttpDelete("{id}") , Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OutputRecordDto>> Delete(int id)
    {
        return Ok(await _mediator.Send(new DeleteRecordCommand(id)));
    }
}