using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand , RegisterUserDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserCommandHandler(AppDbContext context , IMapper mapper , IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<RegisterUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
        var user = _mapper.Map<User>(request);
        
        user.Password = _passwordHasher.HashPassword(user, request.Password);
        
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        user = await _context.User.FirstOrDefaultAsync(u => u.UserId ==  user.UserId);
        
        return _mapper.Map<RegisterUserDto>(user);
    }
}