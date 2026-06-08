namespace Application.Dtos;

public class WebhookSettingsResponseDto
{
    public string WebhookUrl { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
