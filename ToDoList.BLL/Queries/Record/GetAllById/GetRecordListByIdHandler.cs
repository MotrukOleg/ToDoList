using AutoMapper;
using MediatR;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Queries.Record.GetAllById;

public class GetRecordListByIdHandler : IRequestHandler<GetRecordListById, List<OutputRecordDto>>
{
    private readonly IMapper _mapper;
    private readonly IRecordRepository _recordRepository;
    private readonly ITokenService _tokenService;

    public GetRecordListByIdHandler(IRecordRepository recordRepository, IMapper mapper, ITokenService tokenService)
    {
        _recordRepository = recordRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }


    public async Task<List<OutputRecordDto>> Handle(GetRecordListById request, CancellationToken cancellationToken)
    {
        int id = _tokenService.GetCurrentUserId();
        var records = await _recordRepository.GetAllForUserAsync(id);
        return _mapper.Map<List<OutputRecordDto>>(records);
    }
}