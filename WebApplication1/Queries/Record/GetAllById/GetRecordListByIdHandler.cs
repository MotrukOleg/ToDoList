using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Queries.Record.GetAllById;

public class GetRecordListByIdHandler : IRequestHandler<GetRecordListById, List<OutputRecordDto>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public GetRecordListByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<List<OutputRecordDto>> Handle(GetRecordListById request, CancellationToken cancellationToken)
    {
        var records = await _context.Record.Include(r => r.User).Where(r => r.UserId == request.UserId).ToListAsync();
        
        return _mapper.Map<List<OutputRecordDto>>(records);
    }
}