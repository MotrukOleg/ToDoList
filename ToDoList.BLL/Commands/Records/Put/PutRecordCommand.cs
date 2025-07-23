using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Commands.Records.Put;

public class PutRecordCommand : IRequest<OutputRecordDto>
{
    public int RecordId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public string Status { get; set; } = null!;

    public PutRecordCommand(int recordId, string title , string description, string status  , DateOnly? deadline)
    {
        RecordId = recordId;
        Title = title;
        Description = description;
        Status = status;
        Deadline = deadline;
    }
}