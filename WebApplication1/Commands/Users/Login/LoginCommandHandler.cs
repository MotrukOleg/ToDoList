using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.Interfaces;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Commands.Users.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginUserDto>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext  _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginCommandHandler(IMapper mapper, AppDbContext context ,  ITokenService tokenService ,  IPasswordHasher<User> passwordHasher)
    {
        _mapper = mapper;
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<LoginUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        var result = _passwordHasher.VerifyHashedPassword(user , user.Password , request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);

        var dto = _mapper.Map<LoginUserDto>(user);
        dto.Token = token;
        return dto;
    }
}