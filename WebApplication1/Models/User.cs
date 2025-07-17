namespace WebApplication1.Models;

public class User
{
    public int UserId { get; set; }
    public string FistName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Record> Records { get; set; }
}