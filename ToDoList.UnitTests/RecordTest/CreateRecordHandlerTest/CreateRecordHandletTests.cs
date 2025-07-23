using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ToDoList.BLL.Commands.Records.Create;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;
using Record = ToDoList.DAL.Models.Record;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace ToDoList.UnitTests.RecordTest.CreateRecordHandlerTest;

public class CreateRecordHandletTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IRecordRepository> _recordRepositoryMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IValidator<CreateRecordCommand>> _validatorMock = new();
    private readonly CreateRecordCommandHandler _handler;

    public CreateRecordHandletTests()
    {
        _handler = new CreateRecordCommandHandler(
            _mapperMock.Object,
            _recordRepositoryMock.Object,
            _tokenServiceMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsRecordAndReturnsDto()
    {
        var command = new CreateRecordCommand
        {
            Title = "Test record",
            Description = "test description",
            Status = "Pending",
            Deadline = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
        };

        var userId = 123;

        var record = new Record
        {
            RecordId = 1,
            Title = command.Title,
            Description = command.Description,
            Status = command.Status,
            Deadline = command.Deadline,
            UserId = userId
        };

        var expectedDto = new OutputRecordDto
        {
            RecordId = 1,
            Title = command.Title,
            Description = command.Description,
            Status = command.Status,
            Deadline = command.Deadline
        };

        _tokenServiceMock.Setup(t => t.GetCurrentUserId()).Returns(userId);

        _mapperMock.Setup(m => m.Map<Record>(command)).Returns(record);
        _mapperMock.Setup(m => m.Map<OutputRecordDto>(record)).Returns(expectedDto);

        _recordRepositoryMock.Setup(r => r.AddAsync(record, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _recordRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedDto.RecordId, result.RecordId);
        Assert.Equal(expectedDto.Title, result.Title);
        Assert.Equal(expectedDto.Description, result.Description);
        Assert.Equal(expectedDto.Status, result.Status);
        Assert.Equal(expectedDto.Deadline, result.Deadline);

        _recordRepositoryMock.Verify(r => r.AddAsync(record, It.IsAny<CancellationToken>()), Times.Once);
        _recordRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _tokenServiceMock.Verify(t => t.GetCurrentUserId(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRecordTextLength_ThrowsValidationException()
    {
        var command = new CreateRecordCommand
        {
            Title = new string('A', 101),
            Status = "Pending",
            Deadline = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
        };

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title must be less than 100 characters.")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));


        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

    }
}