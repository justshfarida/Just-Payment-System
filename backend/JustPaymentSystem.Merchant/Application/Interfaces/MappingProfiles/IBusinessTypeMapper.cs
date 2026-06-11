using Application.Dtos;
using Domain.Entitites;

namespace Application.Interfaces.MappingProfiles;

public interface IBusinessTypeMapper
{
    BusinessType Map(CreateBusinessTypeDto request);
    BusinessTypeResponseDto MapToResponse(BusinessType businessType);
    void ApplyUpdate(BusinessType businessType, UpdateBusinessTypeDto request);
}
