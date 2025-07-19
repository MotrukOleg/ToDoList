using AutoMapper;
using MediatR;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Records.Put;

public class PutRecordCommandHandler : IRequestHandler<PutRecordCommand, OutputRecordDto>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public PutRecordCommandHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<OutputRecordDto> Handle(PutRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.Record.FindAsync(request.RecordId);

        if (record != null)
        {
            record.RecordText = request.RecordText;

            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<OutputRecordDto>(record);
        }
        return null;
    }
}