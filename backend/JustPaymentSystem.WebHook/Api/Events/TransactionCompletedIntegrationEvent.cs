namespace Api.Events;

public record TransactionCompletedIntegrationEvent(
    Guid TransactionId,
    string MerchantId,
    string OrderId,
    decimal Amount,
    string Currency,
    string SuccessRedirectUrl, 
    string ErrorRedirectUrl,    
    DateTime Timestamp
);