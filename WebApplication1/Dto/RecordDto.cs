using WebApplication1.Models;

namespace WebApplication1.Dto;

public class InputRecordDto
{
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
    public int UserId { get; set; }
}

public class OutputRecordDto
{
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;

    public static explicit operator OutputRecordDto(Record record)
    {
        return new OutputRecordDto
        {
            RecordId = record.RecordId,
            RecordText = record.RecordText,
            Completed = record.Completed,
            UserId = record.User.UserId,
            UserName = record.User.FistName + " " + record.User.LastName
        };
    }
}