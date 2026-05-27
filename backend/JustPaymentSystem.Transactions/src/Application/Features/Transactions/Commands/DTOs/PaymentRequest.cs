namespace Application.Features.Transactions.Commands.DTOs;

public record PaymentRequest
{
    public string CardNumber { get; set; } = null!;
    public string Cvv { get; set; } = null!;
    public int ExpirationMonth { get; init; }
    public int ExpirationYear { get; init; }
}