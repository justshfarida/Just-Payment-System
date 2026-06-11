using Application.Commands.BusinessType;
using Application.Dtos;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Validators.BusinessType;

public class CreateBusinessTypeCommandValidator : AbstractValidator<CreateBusinessTypeCommand>
{
    public CreateBusinessTypeCommandValidator(
        IValidator<CreateBusinessTypeDto> createDtoValidator,
        IBusinessTypeRepository businessTypeRepository)
    {
        RuleFor(x => x.Request)
            .NotNull()
            .SetValidator(createDtoValidator);

        RuleFor(x => x.Request.Name)
            .MustAsync(async (name, cancellationToken) =>
                !await businessTypeRepository.ExistsByNameAsync(name, cancellationToken))
            .WithMessage("A business type with this name already exists.");
    }
}
