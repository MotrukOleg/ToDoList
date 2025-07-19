using MediatR;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Users.Queries;

public class GetUserListQuery : IRequest<List<OutputUserDto>>
{
    public int UserId { get; set; }
    public int RecordId { get; set; }
    public string RecordText { get; set; } = null!;
    public bool Completed { get; set; }
}