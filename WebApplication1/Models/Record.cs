namespace WebApplication1.Models;

public class Record
{
    public int RecordId {get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
}