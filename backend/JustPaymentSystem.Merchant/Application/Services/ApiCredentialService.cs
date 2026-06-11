using Application.Commands.ApiCredential;
using Application.Dtos;
using Application.Helpers;
using Application.Interfaces.MappingProfiles;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entitites;
using FluentValidation;

namespace Application.Services;

public class ApiCredentialService : IApiCredentialService
{
    private readonly IApiCredentialRepository _apiCredentialRepository;
    private readonly IMerchantRepository _merchantRepository;
    private readonly IApiCredentialMapper _mapper;
    private readonly IValidator<UserIdCommand> _userIdValidator;
    private readonly IValidator<CreateApiCredentialCommand> _createValidator;
    private readonly IValidator<UpdateApiCredentialCommand> _updateValidator;
    private readonly IValidator<ApiCredential> _activeValidator;

    public ApiCredentialService(
        IApiCredentialRepository apiCredentialRepository,
        IMerchantRepository merchantRepository,
        IApiCredentialMapper mapper,
        IValidator<UserIdCommand> userIdValidator,
        IValidator<CreateApiCredentialCommand> createValidator,
        IValidator<UpdateApiCredentialCommand> updateValidator,
        IValidator<ApiCredential> activeValidator)
    {
        _apiCredentialRepository = apiCredentialRepository;
        _merchantRepository = merchantRepository;
        _mapper = mapper;
        _userIdValidator = userIdValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _activeValidator = activeValidator;
    }

    public async Task<ApiCredentialResponseDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _userIdValidator.ValidateAndThrowAsync(new UserIdCommand(userId), cancellationToken);

        var apiCredential = await _apiCredentialRepository.GetByUserIdAsync(userId, trackChanges: false);
        return apiCredential is null ? null : _mapper.MapToResponse(apiCredential);
    }

    public async Task<ApiCredentialResponseDto> CreateAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(new CreateApiCredentialCommand(userId), cancellationToken);

        var merchant = await _merchantRepository.GetByUserIdAsync(userId);

        var publicKey = SecretKeyGeneratorHelper.GenerateSecretKey();
        var secretKey = SecretKeyGeneratorHelper.GenerateSecretKey();
        var apiCredential = _mapper.Map(merchant!.Id, publicKey, secretKey);

        await _apiCredentialRepository.CreateAsync(apiCredential);
        await _apiCredentialRepository.SaveChangesAsync();

        return _mapper.MapToResponse(apiCredential);
    }

    public async Task<ApiCredentialResponseDto?> UpdateAsync(Guid userId, UpdateApiCredentialDto request, CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(new UpdateApiCredentialCommand(userId, request), cancellationToken);

        var apiCredential = await _apiCredentialRepository.GetByUserIdAsync(userId, trackChanges: true);
        if (apiCredential is null)
        {
            return null;
        }

        _mapper.ApplyUpdate(apiCredential, request);

        _apiCredentialRepository.Update(apiCredential);
        await _apiCredentialRepository.SaveChangesAsync();

        return _mapper.MapToResponse(apiCredential);
    }

    public async Task<RegenerateSecretKeyResponseDto?> RegenerateSecretKeyAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _userIdValidator.ValidateAndThrowAsync(new UserIdCommand(userId), cancellationToken);

        var apiCredential = await _apiCredentialRepository.GetByUserIdAsync(userId, trackChanges: true);
        if (apiCredential is null)
        {
            return null;
        }

        await _activeValidator.ValidateAndThrowAsync(apiCredential, cancellationToken);

        var secretKey = SecretKeyGeneratorHelper.GenerateSecretKey();
        _mapper.ApplySecretKeyRegeneration(apiCredential, secretKey);

        _apiCredentialRepository.Update(apiCredential);
        await _apiCredentialRepository.SaveChangesAsync();

        return _mapper.MapToRegenerateResponse(secretKey);
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _userIdValidator.ValidateAndThrowAsync(new UserIdCommand(userId), cancellationToken);

        var apiCredential = await _apiCredentialRepository.GetByUserIdAsync(userId, trackChanges: true);
        if (apiCredential is null)
        {
            return false;
        }

        _apiCredentialRepository.Delete(apiCredential);
        await _apiCredentialRepository.SaveChangesAsync();

        return true;
    }
}
