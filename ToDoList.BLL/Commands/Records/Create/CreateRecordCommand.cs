using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Commands.Records.Create;

public class CreateRecordCommand : IRequest<OutputRecordDto>
{
    public int RecordId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateOnly? Deadline { get; set; }
    public int UserId { get; set; }
}