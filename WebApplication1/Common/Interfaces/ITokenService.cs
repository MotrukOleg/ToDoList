using WebApplication1.Models;

namespace WebApplication1.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}