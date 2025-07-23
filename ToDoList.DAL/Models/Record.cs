
namespace ToDoList.DAL.Models;

public class Record
{
    public int RecordId {get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
}