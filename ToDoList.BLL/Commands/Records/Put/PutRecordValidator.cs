using FluentValidation;

namespace ToDoList.BLL.Commands.Records.Put;

public class PutRecordValidator : AbstractValidator<PutRecordCommand>
{
    public PutRecordValidator()
    {
        RuleFor(r => r.Title).MaximumLength(50).NotEmpty();
        RuleFor(r => r.Description).MaximumLength(500);
    }
}