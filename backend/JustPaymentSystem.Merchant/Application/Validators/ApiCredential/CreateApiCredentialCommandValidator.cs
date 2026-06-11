using Application.Commands.ApiCredential;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.ApiCredential;

public class CreateApiCredentialCommandValidator : AbstractValidator<CreateApiCredentialCommand>
{
    public CreateApiCredentialCommandValidator(
        IMerchantRepository merchantRepository,
        IApiCredentialRepository apiCredentialRepository)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await merchantRepository.GetByUserIdAsync(userId) is not null)
            .WithMessage("Merchant not found for the user.");

        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var merchant = await merchantRepository.GetByUserIdAsync(userId);
                if (merchant is null)
                {
                    return true;
                }

                var existingCredential = await apiCredentialRepository.GetByMerchantIdAsync(merchant.Id, trackChanges: false);
                return existingCredential is null;
            })
            .WithMessage("API credentials already exist for this merchant.");
    }
}
