namespace Domain.Entitites;

public class EventType: AuditedEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<WebhookEvent> WebhookEvents { get; set; } = [];
}
