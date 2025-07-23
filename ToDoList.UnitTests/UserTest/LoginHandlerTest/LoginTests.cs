using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using ToDoList.BLL.Commands.Users.Login;
using ToDoList.BLL.Dto;
using ToDoList.DAL.Models;
using ToDoList.DAL.repositories.Interfaces;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace ToDoList.UnitTests.UserTest.LoginHandlerTest;

public class LoginTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
    private readonly Mock<IValidator<LoginCommand>> _validatorMock = new();
    private readonly LoginCommandHandler _handler;

    public LoginTests()
    {
        _handler = new LoginCommandHandler(
            _mapperMock.Object,
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _passwordHasherMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task RequestWithInvalidEmail_ThrowsUnauthorizedAccessException()
    {
        var command = new LoginCommand { Email = "123", Password = "" };
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RequestWithThanEightLengthPassword_ThorowsUnauthorizedAccessException()
    {
        var command = new LoginCommand { Email = "", Password = "" };
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Password", "Password is required")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RequestWithValidEmailAndLessThanEightLengthPassword_ThorowsUnauthorizedAccessException()
    {
        var command = new LoginCommand { Email = "test123@mail.com", Password = "123" };

        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Password", "Password has to be more than  8 characters")
        };


        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));
        
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RequestWithValidEmail_ShouldReturnOk()
    {
        var command = new LoginCommand { Email = "test@mail.com", Password = "123123" };

        var user = new User
        {
            Email = command.Email,
            Password = command.Password
        };

        var expectedToken = "mocked-tocken";

        var expectedDto = new LoginUserDto
        {
            Email = command.Email,
            Token = expectedToken
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.Password, command.Password))
            .Returns(PasswordVerificationResult.Success);

        _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns(expectedToken);

        _mapperMock.Setup(m => m.Map<LoginUserDto>(user)).Returns(new LoginUserDto
        {
            Email = user.Email
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Email, result.Email);
        Assert.Equal(expectedDto.Token, result.Token);
    }
}