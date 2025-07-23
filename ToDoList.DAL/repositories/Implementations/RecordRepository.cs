

using ToDoList.DAL.Data;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.DAL.repositories.Implementations;

public class RecordRepository : BaseRepository<Record>, IRecordRepository
{
    public RecordRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Record>> GetAllForUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Record.Include(r => r.User).Where(r => r.UserId == id).ToListAsync();
    }
}