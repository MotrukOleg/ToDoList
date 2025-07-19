using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Users.Queries;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery , List<OutputUserDto>>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public GetUserListQueryHandler(IMapper mapper , AppDbContext context)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<OutputUserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.User.Include(u => u.Records).ToListAsync(cancellationToken);

        return _mapper.Map<List<OutputUserDto>>(users);
    }
}