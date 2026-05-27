namespace Application.Features.Transactions.IntegrationEvents;

public record PaymentSucceed(Guid TransactionId, string MerchantId);
