using Application.Commands.ApiCredential;
using Application.Dtos;
using FluentValidation;

namespace Application.Validators.ApiCredential;

public class UpdateApiCredentialCommandValidator : AbstractValidator<UpdateApiCredentialCommand>
{
    public UpdateApiCredentialCommandValidator(IValidator<UpdateApiCredentialDto> updateDtoValidator)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(updateDtoValidator);
    }
}
