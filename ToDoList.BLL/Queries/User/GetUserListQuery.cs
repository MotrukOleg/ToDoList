using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Queries.User;

public class GetUserListQuery : IRequest<List<OutputUserDto>>
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
}