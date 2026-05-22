namespace Domain.Entitites;

public class Location : AuditedEntity
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public Guid MerchantId { get; set; }
    public Merchant? Merchant { get; set; }
}
