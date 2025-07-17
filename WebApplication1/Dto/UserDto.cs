namespace WebApplication1.Dto;

public class RegisterUserDto
{
    public string FistName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class OutputUserDto
{
    
}