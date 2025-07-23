using AutoMapper;
using MediatR;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Queries.Record.Get;

public class GetRecordListQueryHandler : IRequestHandler<GetRecordListQuery , List<OutputRecordDto>>
{
    private readonly IRecordRepository _recordRepository;
    private readonly IMapper _mapper;

    public GetRecordListQueryHandler(IRecordRepository recordRepository, IMapper mapper)
    {
        _recordRepository = recordRepository;
        _mapper = mapper;
    } 
    
    public async Task<List<OutputRecordDto>> Handle(GetRecordListQuery request, CancellationToken cancellationToken)
    {
        var records = await _recordRepository.GetAllAsync(cancellationToken);
        
        return  _mapper.Map<List<OutputRecordDto>>(records);
    }
}