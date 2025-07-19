using MediatR;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Users.Login;

public class LoginCommand : IRequest<LoginUserDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
}