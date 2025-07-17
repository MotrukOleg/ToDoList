using MediatR;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Records.Create;

public class CreateRecordCommand : IRequest<OutputRecordDto>
{
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
    public int UserId { get; set; }
}