using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Records.Delete;

public class DeleteRecordCommand : IRequest<OutputRecordDto?>
{
    public int Id { get; set; }

    public DeleteRecordCommand(int id)
    {
        Id = id;
    }
}