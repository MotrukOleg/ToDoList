using ToDoList.DAL.Models;

namespace ToDoList.DAL.repositories.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    int GetCurrentUserId();
}