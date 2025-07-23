using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Queries.Record.GetAllById;

public class GetRecordListById : IRequest<List<OutputRecordDto>>
{
    public int Id { get; set; }
}