namespace Api.Events;

public class WebhookCallbackFailedIntegrationEvent
{
    public Guid TransactionId { get; set; }
    public string MerchantId { get; set; } = string.Empty; // Matches your friend's string format
    public string OrderId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}