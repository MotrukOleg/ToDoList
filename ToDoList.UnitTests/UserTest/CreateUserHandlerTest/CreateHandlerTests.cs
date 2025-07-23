using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using ToDoList.BLL.Commands.Users.Create;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;

namespace ToDoList.UnitTests.UserTest.CreateUserHandlerTest;

public class CreateHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
    private readonly Mock<IValidator<CreateUserCommand>> _validatorMock = new();
    private readonly CreateUserCommandHandler _handler;

    public CreateHandlerTests()
    {
        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task RequestWithValidData_ShouldReturnOk()
    {
        var command = new CreateUserCommand
        {
            FirstName = "Test",
            LastName = "Test2",
            Email = "test@example.com",
            Password = "Password123"
        };
        
        var user = new User { Email = command.Email };
        var dto = new RegisterUserDto { Email = command.Email };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(m => m.Map<User>(command))
            .Returns(user);

        _passwordHasherMock.Setup(h => h.HashPassword(user, command.Password))
            .Returns("hashed-password");

        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        _userRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<RegisterUserDto>(user))
            .Returns(dto);
        
        var result = await _handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(command.Email, result.Email);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>() , CancellationToken.None), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task RequestWithInvalidCommand_ThrowsValidationException()
    {
        var command = new CreateUserCommand
        {
            FirstName = "",
            LastName = "",
            Email = "",
            Password = "short"
        };

        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required."),
            new ValidationFailure("FirstName", "FirstName is required."),
            new ValidationFailure("LastName", "LastName is required."),
            new ValidationFailure("Password", "Password is required.")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));
        
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Email is required", ex.Message);
    }
}