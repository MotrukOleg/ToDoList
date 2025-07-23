using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContext;

    public TokenService(IConfiguration configuration , IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContext = httpContextAccessor;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"] ?? string.Empty));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Token:Issuer"],
            audience: _configuration["Token:Audience"],
            expires: DateTime.Now.AddMinutes(30),
            claims: claims,
            signingCredentials: credential
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public int GetCurrentUserId()
    {
        return int.Parse(_httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }
}