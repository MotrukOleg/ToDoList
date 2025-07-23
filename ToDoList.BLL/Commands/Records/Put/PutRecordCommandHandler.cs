using AutoMapper;
using FluentValidation;
using MediatR;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;


namespace ToDoList.BLL.Commands.Records.Put;

public class PutRecordCommandHandler : IRequestHandler<PutRecordCommand, OutputRecordDto>
{
    private readonly IMapper _mapper;
    private readonly IRecordRepository  _recordRepository;
    private readonly IValidator<PutRecordCommand> _validator;

    public PutRecordCommandHandler(IRecordRepository recordRepository, IMapper mapper , IValidator<PutRecordCommand> validator)
    {
        _recordRepository = recordRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<OutputRecordDto> Handle(PutRecordCommand request,  CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request , cancellationToken);
        if (!(validationResult.IsValid)) throw new ValidationException(validationResult.Errors);
        
        var record = await _recordRepository.GetByIdAsync(request.RecordId , cancellationToken);
        if (record == null)
            throw new KeyNotFoundException("Record not found.");
        
        record.Title = request.Title;
        record.Description = request.Description;
        record.Status = request.Status;
        record.Deadline = request.Deadline;
        
        _recordRepository.Update(record);
        await _recordRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OutputRecordDto>(record);
    }
}