using FluentValidation;
using ApiCredentialEntity = Domain.Entitites.ApiCredential;

namespace Application.Validators.ApiCredential;

public class ApiCredentialActiveValidator : AbstractValidator<ApiCredentialEntity>
{
    public ApiCredentialActiveValidator()
    {
        RuleFor(x => x.IsActive)
            .Equal(true)
            .WithMessage("API credentials are inactive.");
    }
}
