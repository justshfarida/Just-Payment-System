using Application.Commands.ApiCredential;
using FluentValidation;

namespace Application.Validators.ApiCredential;

public class UserIdCommandValidator : AbstractValidator<UserIdCommand>
{
    public UserIdCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}
