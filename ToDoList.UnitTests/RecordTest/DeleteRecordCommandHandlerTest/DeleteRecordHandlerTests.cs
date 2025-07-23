
using AutoMapper;
using Moq;
using ToDoList.BLL.Commands.Records.Delete;
using ToDoList.BLL.Dto;
using ToDoList.DAL.repositories.Interfaces;
using Record = ToDoList.DAL.Models.Record;

namespace ToDoList.UnitTests.RecordTest.DeleteRecordCommandHandlerTest
{
    public class DeleteRecordHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IRecordRepository> _recordRepositoryMock = new();
        private readonly DeleteRecordCommandHandler _handler;

        public DeleteRecordHandlerTests()
        {
            _handler = new DeleteRecordCommandHandler(
                _recordRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task RequestValidData_ShouldReturnOutputRecordDto()
        {
            var recordId = 1;

            var record = new Record
            {
                RecordId = 1,
                Title = "Test record",
                Description = "Test description",
                Status = "Todo",
                Deadline = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
            };

            _recordRepositoryMock.Setup(r => r.GetByIdAsync(recordId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(record);

            _recordRepositoryMock.Setup(r => r.Delete(record));

            _recordRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<OutputRecordDto>(record))
                .Returns(new OutputRecordDto
                {
                    RecordId = record.RecordId,
                    Title = record.Title,
                    Description = record.Description,
                    Status = record.Status,
                    Deadline = record.Deadline,
                    UserId = 15,
                    UserName = null
                });

            var result = await _handler.Handle(new DeleteRecordCommand(recordId) , CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(record.RecordId, recordId);
            _recordRepositoryMock.Verify(r => r.GetByIdAsync(recordId, It.IsAny<CancellationToken>()), Times.Once);
            _recordRepositoryMock.Verify(r => r.Delete(record), Times.Once);
            _recordRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task RequestInvalidData_ShouldReturnNull()
        {
            var recordId = 2;

            _recordRepositoryMock.Setup(r => r.GetByIdAsync(recordId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Record?)null);

            await Assert.ThrowsAsync<InvalidDataException>(
                () => _handler.Handle(new DeleteRecordCommand(recordId), CancellationToken.None));

            _mapperMock.Verify(m => m.Map<OutputRecordDto>(It.IsAny<Record>()), Times.Never);
            _recordRepositoryMock.Verify(r => r.Delete(It.IsAny<Record>()), Times.Never);
            _recordRepositoryMock.Verify(r => r.GetByIdAsync(recordId, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
