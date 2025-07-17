using MediatR;
using WebApplication1.Dto;

namespace WebApplication1.Queries.Record.GetAllById;

public class GetRecordListById : IRequest<List<OutputRecordDto>>
{
    public int UserId { get; set; }
    public GetRecordListById(int id)
    {
        UserId = id;
    }
    
}