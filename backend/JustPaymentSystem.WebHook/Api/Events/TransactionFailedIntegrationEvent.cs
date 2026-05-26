namespace Api.Events;

    public record TransactionFailedIntegrationEvent(

        Guid TransactionId,
        string MerchantId,
        string OrderId,
        string Reason,
        DateTime Timestamp
    );

