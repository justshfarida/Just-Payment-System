using FluentValidation;

namespace Application.Features.Transactions.Commands.Validations;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(c => c.Amount)
            .GreaterThan(0);

        RuleFor(c => c.Currency)
            .Length(3);

        RuleFor(c => c.MerchantId)
            .NotEmpty();

        RuleFor(c => c.IdempotencyKey)
            .NotEmpty();

        RuleFor(c => c.OrderId)
            .NotEmpty();
    }
}
