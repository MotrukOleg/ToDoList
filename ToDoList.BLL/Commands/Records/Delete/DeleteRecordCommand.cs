

using MediatR;
using ToDoList.BLL.Dto;

namespace ToDoList.BLL.Commands.Records.Delete;

public class DeleteRecordCommand : IRequest<OutputRecordDto?>
{
    public int Id { get; set; }

    public DeleteRecordCommand(int id)
    {
        Id = id;
    }
}