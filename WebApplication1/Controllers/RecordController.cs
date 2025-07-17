using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;


[Route("api/record")]
[ApiController]
public class RecordController : ControllerBase
{
    private readonly RecordRepository _recordRepository;

    public RecordController(RecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    [HttpGet , Authorize]
    public async Task<ActionResult<List<OutputRecordDto>>> Get()
    {
        List<OutputRecordDto> records = await _recordRepository.GetAll();

        return Ok(records);
    }

    [HttpGet("{id}") , Authorize]
    public async Task<ActionResult<OutputRecordDto>> GetById(int id)
    {
        var record = await _recordRepository.GetById(id);
        
        return Ok(record);
    }

    [HttpGet("/GetAllByUserId/{id}"), Authorize]
    public async Task<ActionResult<List<OutputRecordDto?>>> GetAllByUserId(int id)
    {
        List<OutputRecordDto?> records = await _recordRepository.GetAll();
        return Ok(records);
    }

    [HttpPost , Authorize]
    public async Task<ActionResult<OutputRecordDto>> Post([FromBody] InputRecordDto? record)
    {
        if(record == null) return BadRequest();
        
        OutputRecordDto? newRecord = await _recordRepository.Create(record);

        return Created();
    }

    
    [HttpPut("{id}") , Authorize]
    public async Task<ActionResult<OutputRecordDto>> Put(int id, [FromBody] InputRecordDto? record)
    {
        if(record == null) return BadRequest();
        
        OutputRecordDto? updatedRecord = await _recordRepository.Put(record);
        if (updatedRecord == null)
        {
            return NotFound();
        }

        return Ok(updatedRecord);
    }

    [HttpDelete("{id}") , Authorize]
    public async Task<ActionResult<OutputRecordDto>> Delete(int id)
    {
        var deletedRecord = await _recordRepository.Delete(id);
        
        return deletedRecord ? NoContent() : NotFound();
    }
}