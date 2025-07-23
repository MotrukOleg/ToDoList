using AutoMapper;
using MediatR;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Commands.Records.Delete;

public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, OutputRecordDto?>
{
    private readonly IMapper _mapper;
    private readonly IRecordRepository _recordRepository;

    public DeleteRecordCommandHandler(IRecordRepository recordRepository, IMapper mapper)
    {
        _recordRepository = recordRepository;
        _mapper = mapper;
    }
    
    public async Task<OutputRecordDto?> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetByIdAsync(request.Id , cancellationToken);
        
        _recordRepository.Delete(record);
        await _recordRepository.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<OutputRecordDto>(record);
    }
}