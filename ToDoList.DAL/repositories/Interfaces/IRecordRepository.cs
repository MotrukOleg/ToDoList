using ToDoList.DAL.Models;

namespace ToDoList.DAL.repositories.Interfaces;

public interface IRecordRepository : IBaseRepository<Record>
{
    Task<IEnumerable<Record>> GetAllForUserAsync(int id,CancellationToken cancellationToken = default);
}