using FluentValidation;

namespace WebApplication1.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}