using AutoMapper;
using FluentValidation;
using MediatR;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Commands.Records.Create;

public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand, OutputRecordDto>
{
    private readonly IMapper _mapper;
    private readonly IRecordRepository _recordRepository;
    private readonly ITokenService _tokenService;
    private readonly IValidator<CreateRecordCommand> _validator;

    public CreateRecordCommandHandler(IMapper mapper, IRecordRepository recordRepository , ITokenService tokenService ,  IValidator<CreateRecordCommand> validator)
    {
        _mapper = mapper;
        _recordRepository = recordRepository;
        _tokenService = tokenService;
        _validator = validator;
    }
    
    public async Task<OutputRecordDto> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request , cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        request.UserId = _tokenService.GetCurrentUserId();
        var record = _mapper.Map<Record>(request);

        await _recordRepository.AddAsync(record , cancellationToken);
        await _recordRepository.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<OutputRecordDto>(record);
    }
    
}