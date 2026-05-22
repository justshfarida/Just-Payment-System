namespace Domain.Entitites;

public class Contact : AuditedEntity
{
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Guid MerchantId { get; set; }
    public Merchant? Merchant { get; set; }
}
