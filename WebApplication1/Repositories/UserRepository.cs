using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepository(AppDbContext context , IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    public async Task<User?> Create(RegisterUserDto entity)
    {
        User? user = new User
        {
            Email = entity.Email,
            Password = entity.Password,
            FistName = entity.FistName,
            LastName = entity.LastName,
        };

        _context.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<(User? , string?)> Login(LoginUserDto entity)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == entity.Email && u.Password == entity.Password);
        if (user == null) return (null , string.Empty);
        
        var token = GenerateToken(user);
        return (user , token);
    }

    public async Task<List<User?>> GetAll()
    {
        List<User> users = await _context.User.ToListAsync();

        return users;
    }
    

    public async Task<User?> GetById(int id)
    {
        User? user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);

        return user;
    }

    public async Task<User?> Put(RegisterUserDto entity)
    {
        User? user = await _context.User.FirstOrDefaultAsync(u => u.Email == entity.Email);

        user.Email = entity.Email;
        user.Password = entity.Password;
        user.FistName = entity.FistName;
        user.LastName = entity.LastName;

        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> Delete(int id)
    {
        User? user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return false;

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }
    
    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"] ?? string.Empty));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FistName)
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
}