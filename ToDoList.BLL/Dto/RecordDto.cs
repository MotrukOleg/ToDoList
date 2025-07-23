
namespace ToDoList.BLL.Dto;

public class InputRecordDto
{
    public int RecordId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int UserId { get; set; }
}


public class OutputRecordDto
{
    public int RecordId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
}