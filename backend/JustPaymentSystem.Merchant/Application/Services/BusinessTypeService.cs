using Application.Commands.BusinessType;
using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using FluentValidation;

namespace Application.Services;

public class BusinessTypeService : IBusinessTypeService
{
    private readonly IBusinessTypeRepository _businessTypeRepository;
    private readonly IBusinessTypeMapper _mapper;
    private readonly IValidator<CreateBusinessTypeCommand> _createValidator;
    private readonly IValidator<UpdateBusinessTypeCommand> _updateValidator;
    private readonly IValidator<DeleteBusinessTypeCommand> _deleteValidator;
    private readonly IValidator<BusinessTypeIdCommand> _idValidator;

    public BusinessTypeService(
        IBusinessTypeRepository businessTypeRepository,
        IBusinessTypeMapper mapper,
        IValidator<CreateBusinessTypeCommand> createValidator,
        IValidator<UpdateBusinessTypeCommand> updateValidator,
        IValidator<DeleteBusinessTypeCommand> deleteValidator,
        IValidator<BusinessTypeIdCommand> idValidator)
    {
        _businessTypeRepository = businessTypeRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _deleteValidator = deleteValidator;
        _idValidator = idValidator;
    }

    public async Task<IReadOnlyList<BusinessTypeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var businessTypes = await _businessTypeRepository.GetAllAsync(cancellationToken);
        return businessTypes.Select(_mapper.MapToResponse).ToList();
    }

    public async Task<BusinessTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _idValidator.ValidateAndThrowAsync(new BusinessTypeIdCommand(id), cancellationToken);

        var businessType = await _businessTypeRepository.GetByIdAsync(id, trackChanges: false);
        return businessType is null ? null : _mapper.MapToResponse(businessType);
    }

    public async Task<BusinessTypeResponseDto> CreateAsync(CreateBusinessTypeDto request, CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(new CreateBusinessTypeCommand(request), cancellationToken);

        var businessType = _mapper.Map(request);

        await _businessTypeRepository.CreateAsync(businessType);
        await _businessTypeRepository.SaveChangesAsync();

        return _mapper.MapToResponse(businessType);
    }

    public async Task<BusinessTypeResponseDto?> UpdateAsync(Guid id, UpdateBusinessTypeDto request, CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(new UpdateBusinessTypeCommand(id, request), cancellationToken);

        var businessType = await _businessTypeRepository.GetByIdAsync(id, trackChanges: true);
        if (businessType is null)
        {
            return null;
        }

        _mapper.ApplyUpdate(businessType, request);

        _businessTypeRepository.Update(businessType);
        await _businessTypeRepository.SaveChangesAsync();

        return _mapper.MapToResponse(businessType);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _deleteValidator.ValidateAndThrowAsync(new DeleteBusinessTypeCommand(id), cancellationToken);

        var businessType = await _businessTypeRepository.GetByIdAsync(id, trackChanges: true);
        if (businessType is null)
        {
            return false;
        }

        _businessTypeRepository.Delete(businessType);
        await _businessTypeRepository.SaveChangesAsync();

        return true;
    }
}
