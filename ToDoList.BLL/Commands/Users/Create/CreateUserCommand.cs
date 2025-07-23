using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Commands.Users.Create;

public class CreateUserCommand : IRequest<RegisterUserDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}