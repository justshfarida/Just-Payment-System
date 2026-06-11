using Application.Dtos;
using Domain.Entitites;

namespace Application.Interfaces.MappingProfiles;

public interface IApiCredentialMapper
{
    ApiCredential Map(Guid merchantId, string publicKey, string secretKey);
    ApiCredentialResponseDto MapToResponse(ApiCredential apiCredential);
    RegenerateSecretKeyResponseDto MapToRegenerateResponse(string secretKey);
    void ApplyUpdate(ApiCredential apiCredential, UpdateApiCredentialDto request);
    void ApplySecretKeyRegeneration(ApiCredential apiCredential, string secretKey);
}
