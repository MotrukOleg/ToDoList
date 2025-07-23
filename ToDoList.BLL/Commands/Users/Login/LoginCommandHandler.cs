
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.BLL.Commands.Users.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginUserDto>
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<LoginCommand> _validator;

    public LoginCommandHandler(IMapper mapper, IUserRepository userRepository ,  ITokenService tokenService ,  IPasswordHasher<User> passwordHasher , IValidator<LoginCommand> validator)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }
    
    public async Task<LoginUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken); 
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        var result = _passwordHasher.VerifyHashedPassword(user , user.Password , request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);

        var dto = _mapper.Map<LoginUserDto>(user);
        dto.Token = token;
        return dto;
    }
}