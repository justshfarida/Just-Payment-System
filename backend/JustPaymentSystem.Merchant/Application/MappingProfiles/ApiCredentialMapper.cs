using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Domain.Entitites;

namespace Application.MappingProfiles;

public class ApiCredentialMapper : IApiCredentialMapper
{
    public ApiCredential Map(Guid merchantId, string publicKey, string secretKey)
    {
        var now = DateTime.UtcNow;

        return new ApiCredential
        {
            Id = Guid.NewGuid(),
            MerchantId = merchantId,
            PublicKey = publicKey,
            SecretKeyHash = secretKey,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public ApiCredentialResponseDto MapToResponse(ApiCredential apiCredential)
    {
        return new ApiCredentialResponseDto
        {
            MerchantId = apiCredential.MerchantId,
            PublicKey = apiCredential.PublicKey,
            SecretKey = apiCredential.SecretKeyHash,
            IsActive = apiCredential.IsActive
        };
    }

    public RegenerateSecretKeyResponseDto MapToRegenerateResponse(string secretKey)
    {
        return new RegenerateSecretKeyResponseDto
        {
            SecretKey = secretKey
        };
    }

    public void ApplyUpdate(ApiCredential apiCredential, UpdateApiCredentialDto request)
    {
        apiCredential.IsActive = request.IsActive;
        apiCredential.UpdatedAt = DateTime.UtcNow;
    }

    public void ApplySecretKeyRegeneration(ApiCredential apiCredential, string secretKey)
    {
        apiCredential.SecretKeyHash = secretKey;
        apiCredential.UpdatedAt = DateTime.UtcNow;
    }
}
