namespace Domain.Entitites;

public class BusinessType: AuditedEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Merchant>? Merchants { get; set; } = [];
}
