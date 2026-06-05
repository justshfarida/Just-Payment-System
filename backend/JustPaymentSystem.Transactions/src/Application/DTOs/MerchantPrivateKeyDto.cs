namespace Application.DTOs;

public class MerchantPrivateKeyDto
{
    public string WebhookUrl { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}