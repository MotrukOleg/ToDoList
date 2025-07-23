using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.BLL.Commands.Records.Create;
using ToDoList.BLL.Commands.Records.Delete;
using ToDoList.BLL.Commands.Records.Put;
using ToDoList.BLL.Dto;
using ToDoList.BLL.Queries.Record.Get;
using ToDoList.BLL.Queries.Record.GetAllById;
using ToDoList.DAL.Models;

namespace ToDoList.WebApi.Controllers;


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

    [HttpGet("AllForUser"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById()
    {
        return Ok(await _mediator.Send(new GetRecordListById()));
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
    public async Task<IActionResult> Put(int id, [FromBody] PutRecordCommand? record)
    {
        if(record == null) return BadRequest();
        record.RecordId = id;
        
        return Ok(await _mediator.Send(record));
    }

    [HttpDelete("{id}") , Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OutputRecordDto>> Delete(int id)
    {
        return Ok(await _mediator.Send(new DeleteRecordCommand(id)));
    }
}