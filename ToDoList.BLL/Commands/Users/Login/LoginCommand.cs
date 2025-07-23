using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Commands.Users.Login;

public class LoginCommand : IRequest<LoginUserDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    
}