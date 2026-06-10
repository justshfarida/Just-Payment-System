namespace Application.Dtos;

public class ApiCredentialResponseDto
{
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKeyHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}