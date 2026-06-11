using Application.Commands.BusinessType;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class DeleteBusinessTypeCommandValidator : AbstractValidator<DeleteBusinessTypeCommand>
{
    public DeleteBusinessTypeCommandValidator(IBusinessTypeRepository businessTypeRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Business type ID is required.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await businessTypeRepository.GetByIdAsync(id, trackChanges: false) is not null)
            .WithMessage("Business type not found.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                !await businessTypeRepository.HasMerchantsAsync(id, cancellationToken))
            .WithMessage("Business type cannot be deleted because it is linked to one or more merchants.");
    }
}
