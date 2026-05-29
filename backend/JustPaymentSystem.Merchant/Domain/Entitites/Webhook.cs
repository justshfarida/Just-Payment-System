namespace Domain.Entitites;

public class Webhook : AuditedEntity
{
    public string WebhookUrl { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Guid MerchantId { get; set; }
    public Merchant? Merchant { get; set; }

    public ICollection<WebhookEvent>? WebhookEvents { get; set; } = [];
}
