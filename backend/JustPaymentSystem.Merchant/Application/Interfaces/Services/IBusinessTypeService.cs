using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IBusinessTypeService
{
    Task<IReadOnlyList<BusinessTypeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BusinessTypeResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BusinessTypeResponseDto> CreateAsync(CreateBusinessTypeDto request, CancellationToken cancellationToken = default);
    Task<BusinessTypeResponseDto?> UpdateAsync(Guid id, UpdateBusinessTypeDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
