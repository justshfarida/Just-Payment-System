using Application.Features.Transactions.Commands.DTOs;
using FluentValidation;

namespace Application.Features.Transactions.Commands.Validations;

public class PaymentRequestDtoValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestDtoValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .CreditCard().WithMessage("Invalid credit card number format.")
            .Length(13, 19).WithMessage("Card number must be between 13 and 19 digits.");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required.")
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be exactly 3 or 4 digits.");

        RuleFor(x => x.ExpirationMonth)
            .InclusiveBetween(1, 12).WithMessage("Expiration month must be between 1 and 12.");

        RuleFor(x => x.ExpirationYear)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage($"Expiration year cannot be in the past. Minimum allowed is {DateTime.UtcNow.Year}.");

        RuleFor(x => x)
            .Must(BeAValidExpirationDate)
            .WithMessage("The credit card has expired.")
            .WithName("ExpirationDate"); 
    }

    private bool BeAValidExpirationDate(PaymentRequest dto)
    {
        if (dto.ExpirationYear < DateTime.UtcNow.Year || dto.ExpirationMonth < 1 || dto.ExpirationMonth > 12)
        {
            return false;
        }
        
        var lastDayOfExpirationMonth = new DateTime(dto.ExpirationYear, dto.ExpirationMonth, 1)
            .AddMonths(1)
            .AddDays(-1);

        return lastDayOfExpirationMonth.Date >= DateTime.UtcNow.Date;
    }
}