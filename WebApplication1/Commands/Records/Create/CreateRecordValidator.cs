using FluentValidation;

namespace WebApplication1.Commands.Records.Create;

public class CreateRecordValidator : AbstractValidator<CreateRecordCommand>
{
    public CreateRecordValidator()
    {
        RuleFor(x => x.RecordText).NotEmpty().MaximumLength(100);
    }
}