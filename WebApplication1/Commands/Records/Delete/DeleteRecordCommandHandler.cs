using AutoMapper;
using MediatR;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Commands.Records.Delete;

public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, OutputRecordDto?>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public DeleteRecordCommandHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<OutputRecordDto?> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.Record.FindAsync(request.Id);
        
        _context.Remove(record);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<OutputRecordDto>(record);
    }
}