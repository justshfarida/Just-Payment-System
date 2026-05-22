using Domain.Enums;

namespace Domain.Entitites;

public class Merchant : AuditedEntity
{
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; }
    public string VOEN { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public Guid TypeId { get; set; }
    public BusinessType? Type { get; set; }

    public Location? Location { get; set; }
    public Contact? Contact { get; set; }
    public ApiCredential? ApiCredential { get; set; }
    public Webhook? Webhook { get; set; }
}
