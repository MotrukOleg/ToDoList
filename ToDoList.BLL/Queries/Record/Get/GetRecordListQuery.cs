using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Queries.Record.Get;

public class GetRecordListQuery : IRequest<List<OutputRecordDto>>
{
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
}