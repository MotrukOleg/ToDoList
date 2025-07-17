using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Queries.Record;

public class GetRecordListQueryHandler : IRequestHandler<GetRecordListQuery , List<OutputRecordDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetRecordListQueryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    } 
    
    public async Task<List<OutputRecordDto>> Handle(GetRecordListQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.Record.Include(u => u.User).ToListAsync();
        
        return  _mapper.Map<List<OutputRecordDto>>(records);
    }
}