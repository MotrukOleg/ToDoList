using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ToDoList.BLL.Commands.Records.Put;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;
using Record = ToDoList.DAL.Models.Record;

namespace ToDoList.UnitTests.RecordTest.PutRecordHandlerTest;

public class PutRecordCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IRecordRepository> _recordRepositoryMock = new();
    private readonly Mock<IValidator<PutRecordCommand>> _validatorMock = new();
    private readonly PutRecordCommandHandler _handler;

    public PutRecordCommandHandlerTests()
    {
        _handler = new PutRecordCommandHandler(
            _recordRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
    }
    
    [Fact]
    public async Task ValidCommand_UpdatesRecordAndReturnsDto()
    {
        var command = new PutRecordCommand(1, "Updated text", "new description","Done", DateOnly.FromDateTime(DateTime.Today));

        var record = new Record { RecordId = 1, Title = "Old", Description = "old description" , Status = "Pending" };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _recordRepositoryMock.Setup(r => r.GetByIdAsync(command.RecordId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        _mapperMock.Setup(m => m.Map<OutputRecordDto>(It.IsAny<Record>()))
            .Returns(new OutputRecordDto { RecordId = 1, Title = command.Title , Description = command.Description});
        
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        _recordRepositoryMock.Verify(r => r.Update(record), Times.Once);
       _recordRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task InvalidCommand_ThrowsValidationException()
    {
        var command = new PutRecordCommand(1 , null , null ,null , null);
        var validationFailures = new List<ValidationFailure>
        {
            new(nameof(PutRecordCommand.Title), "RecordText is required")
        };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));
        
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task RecordNotFound_ThrowsKeyNotFoundException()
    {
        var command = new PutRecordCommand(999, "Text", "Pending", null , null);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _recordRepositoryMock.Setup(r => r.GetByIdAsync(command.RecordId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Record?)null);
        
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Record not found.", ex.Message);
    }
}
