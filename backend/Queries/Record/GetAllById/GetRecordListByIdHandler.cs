using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.Interfaces;
using WebApplication1.Data;
using WebApplication1.Dto;

namespace WebApplication1.Queries.Record.GetAllById;

public class GetRecordListByIdHandler : IRequestHandler<GetRecordListById, List<OutputRecordDto>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly ITokenService  _tokenService;

    public GetRecordListByIdHandler(AppDbContext context, IMapper mapper , ITokenService tokenService)
    {
        _context = context;
        _mapper = mapper;
        _tokenService = tokenService;
    }


    public async Task<List<OutputRecordDto>> Handle(GetRecordListById request, CancellationToken cancellationToken)
    {
        int id = _tokenService.GetCurrentUserId();
        request.UserId = id;
        var records = await _context.Record.Include(r => r.User).Where(r => r.UserId == request.UserId).ToListAsync();
        
        return _mapper.Map<List<OutputRecordDto>>(records);
    }
}