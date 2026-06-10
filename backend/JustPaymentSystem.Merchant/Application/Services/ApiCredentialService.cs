using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class ApiCredentialService : IApiCredentialService
{
    private readonly IApiCredentialRepository _repository;
    public ApiCredentialService(IApiCredentialRepository repository)
    {
        _repository = repository;
    }
    public async Task<ApiCredentialResponseDto?> GetApiCredentialsByMerchantIdAsync(Guid merchantId)
    {
        var apiCredential = await _repository.GetByMerchantIdAsync(merchantId);
        if (apiCredential == null) return null;
        return new ApiCredentialResponseDto
        {
            SecretKeyHash = apiCredential.SecretKeyHash
        };
    }

    public async Task<ApiCredentialResponseDto?> GetApiCredentialsByUserIdAsync(Guid userId)
    {
        var apiCredential = await _repository.GetByUserIdAsync(userId);
        if (apiCredential == null) return null;
        return new ApiCredentialResponseDto
        {
            SecretKeyHash = apiCredential.SecretKeyHash
        };
    }
    //public async Task GenerateSecretKeyAsync(Guid userId)
    //{
    //    var apiCredential = await _repository.GetByUserIdAsync(userId);
    //    if (apiCredential == null)
    //    {
    //        throw new Exception("API credentials not found for the user.");
    //    }
    //    var newSecretKeyHash = HashingHelper.HashSecretKey(Guid.NewGuid().ToString());
    //    apiCredential.SecretKeyHash = newSecretKeyHash;
    //    await _repository.UpdateAsync(apiCredential);
    //}
}