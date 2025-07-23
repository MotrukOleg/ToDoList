

namespace ToDoList.BLL.Dto;

public class RegisterUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserDto
{
    public string Email { get; set; }
    public string Token { get; set; } 
}

public class OutputUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; }  = null!;
    public string Password { get; set; }  = null!;
    public List<OutputRecordDto> Records { get; set; } = new();
}