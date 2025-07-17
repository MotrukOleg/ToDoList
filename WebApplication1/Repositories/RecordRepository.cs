using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class RecordRepository
{
    private readonly AppDbContext _context;

    public RecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OutputRecordDto?> Create(InputRecordDto entity)
    {
        Record? record = new Record
        {
            RecordId = entity.RecordId,
            RecordText = entity.RecordText,
            Completed = entity.Completed,
            UserId = entity.UserId,
        };


        _context.Add(record);
        await _context.SaveChangesAsync();

        record = await _context.Record.Include(r => r.User).FirstOrDefaultAsync(r => r.RecordId == record.RecordId);

        return (OutputRecordDto)record;
    }


    public async Task<List<OutputRecordDto?>> GetAll()
    {
        List<Record> records = await _context.Record.Include(r => r.User).ToListAsync();

        List<OutputRecordDto> outputRecords = records.Select(r => (OutputRecordDto)r).ToList();

        return outputRecords;
    }

    public async Task<OutputRecordDto?> GetById(int id)
    {
        Record? record = await _context.Record.Include(r => r.User).FirstOrDefaultAsync(r => r.RecordId == id);

        return (OutputRecordDto)record;
    }

    public async Task<List<OutputRecordDto?>> GetAllByUserId(int id)
    {
        List<Record> record = await _context.Record.Include(r => r.User).Where(r => r.UserId == id).ToListAsync();
        List<OutputRecordDto> outputRecords = record.Select(r => (OutputRecordDto)r).ToList();

        return outputRecords;
    }

    public async Task<OutputRecordDto?> Put(InputRecordDto? entity)
    {
        Record? record = await _context.Record.FirstOrDefaultAsync(r => r.RecordId == entity.RecordId);
        if (record == null) return null;


        record.RecordText = entity.RecordText;
        record.Completed = entity.Completed;
        record.UserId = entity.UserId;

        _context.Entry(record).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        record = await _context.Record.Include(r => r.User).FirstOrDefaultAsync(r => r.RecordId == record.RecordId);

        return (OutputRecordDto)record;
    }

    public async Task<bool> Delete(int id)
    {
        Record? record = _context.Record.FirstOrDefault(r => r.RecordId == id);
        if (record == null) return false;

        _context.Record.Remove(record);

        await _context.SaveChangesAsync();
        return true;
    }
}