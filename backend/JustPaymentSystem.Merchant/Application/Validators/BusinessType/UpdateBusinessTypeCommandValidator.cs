using Application.Commands.BusinessType;
using Application.Dtos;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class UpdateBusinessTypeCommandValidator : AbstractValidator<UpdateBusinessTypeCommand>
{
    public UpdateBusinessTypeCommandValidator(
        IValidator<UpdateBusinessTypeDto> updateDtoValidator,
        IBusinessTypeRepository businessTypeRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Business type ID is required.");

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await businessTypeRepository.GetByIdAsync(id, trackChanges: false) is not null)
            .WithMessage("Business type not found.");

        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(updateDtoValidator);

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                !await businessTypeRepository.ExistsByNameAsync(command.Request.Name, command.Id, cancellationToken))
            .WithMessage("A business type with this name already exists.");
    }
}
