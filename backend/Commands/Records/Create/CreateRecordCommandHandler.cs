using System.Security.Claims;
using AutoMapper;
using MediatR;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Commands.Records.Create;

public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand, OutputRecordDto>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _http_context;

    public CreateRecordCommandHandler(IMapper mapper, AppDbContext context , IHttpContextAccessor httpContext)
    {
        _mapper = mapper;
        _context = context;
        _http_context = httpContext;
    }
    
    public async Task<OutputRecordDto> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
    {
        request.UserId = GetCurrentUserId();
        var record = _mapper.Map<Record>(request);

        await _context.Record.AddAsync(record);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<OutputRecordDto>(record);
    }
    
    private int GetCurrentUserId()
    {
        return int.Parse(_http_context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }
}