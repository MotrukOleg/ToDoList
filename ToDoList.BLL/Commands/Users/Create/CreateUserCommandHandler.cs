using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Commands.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand , RegisterUserDto>
{
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IUserRepository userRepository , IMapper mapper , IPasswordHasher<User> passwordHasher ,  IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }
    
    public async Task<RegisterUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
        var user = _mapper.Map<User>(request);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!(validationResult.IsValid))
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        user.Password = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        return _mapper.Map<RegisterUserDto>(user);
    }
}