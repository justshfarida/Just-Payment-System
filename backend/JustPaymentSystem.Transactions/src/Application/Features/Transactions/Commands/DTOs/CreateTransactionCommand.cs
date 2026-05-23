namespace Application.Features.Transactions.Commands.DTOs;

public record CreateTransactionCommand(
    string MerchantId,
    decimal Amount,
    string Currency,
    string OrderId,
    string Description,
    string? SuccessRedirectUrl,
    string? ErrorRedirectUrl,
    string[] OtherAttr
    );
