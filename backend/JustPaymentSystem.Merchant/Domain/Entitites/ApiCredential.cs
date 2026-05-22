namespace Domain.Entitites;

public class  ApiCredential : AuditedEntity
{
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKeyHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Guid MerchantId { get; set; }
    public Merchant? Merchant { get; set; }
}
