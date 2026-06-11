using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Domain.Entitites;

namespace Application.MappingProfiles;

public class BusinessTypeMapper : IBusinessTypeMapper
{
    public BusinessType Map(CreateBusinessTypeDto request)
    {
        var now = DateTime.UtcNow;

        return new BusinessType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public BusinessTypeResponseDto MapToResponse(BusinessType businessType)
    {
        return new BusinessTypeResponseDto
        {
            Id = businessType.Id,
            Name = businessType.Name,
            CreatedAt = businessType.CreatedAt,
            UpdatedAt = businessType.UpdatedAt
        };
    }

    public void ApplyUpdate(BusinessType businessType, UpdateBusinessTypeDto request)
    {
        businessType.Name = request.Name;
        businessType.UpdatedAt = DateTime.UtcNow;
    }
}
