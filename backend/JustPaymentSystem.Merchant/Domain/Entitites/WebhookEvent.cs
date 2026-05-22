namespace Domain.Entitites;

public class WebhookEvent: AuditedEntity
{
    public Guid WebhookId { get; set; }
    public Webhook? Webhook { get; set; }
    public Guid EventId { get; set; }
    public EventType? Event { get; set; }
}