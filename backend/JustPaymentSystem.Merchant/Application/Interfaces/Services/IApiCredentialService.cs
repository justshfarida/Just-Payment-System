using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IApiCredentialService
{
    Task<ApiCredentialResponseDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ApiCredentialResponseDto> CreateAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ApiCredentialResponseDto?> UpdateAsync(Guid userId, UpdateApiCredentialDto request, CancellationToken cancellationToken = default);
    Task<RegenerateSecretKeyResponseDto?> RegenerateSecretKeyAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
}
