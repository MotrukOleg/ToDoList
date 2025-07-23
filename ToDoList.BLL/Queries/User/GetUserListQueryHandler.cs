using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Data;

namespace ToDoList.BLL.Queries.User;

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