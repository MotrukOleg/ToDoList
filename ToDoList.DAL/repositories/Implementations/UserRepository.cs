using Microsoft.EntityFrameworkCore;
using ToDoList.DAL.Data;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;


namespace ToDoList.DAL.repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) {}

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.User.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}