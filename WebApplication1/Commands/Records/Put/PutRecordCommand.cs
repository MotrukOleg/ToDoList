using MediatR;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Records.Put;

public class PutRecordCommand : IRequest<OutputRecordDto>
{
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;

    public PutRecordCommand(int recordId, string recordText)
    {
        RecordId = recordId;
        RecordText = recordText;
    }
}