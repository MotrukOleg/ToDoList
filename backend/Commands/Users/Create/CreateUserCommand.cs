using MediatR;
using WebApplication1.Dto;

namespace WebApplication1.Users.Commands;

public class CreateUserCommand : IRequest<RegisterUserDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}