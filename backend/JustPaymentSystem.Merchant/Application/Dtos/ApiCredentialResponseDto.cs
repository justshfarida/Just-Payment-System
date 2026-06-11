namespace Application.Dtos;

public class ApiCredentialResponseDto
{
    public Guid MerchantId { get; set; }
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
