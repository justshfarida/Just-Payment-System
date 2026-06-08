using Application.Dtos;
using Domain.Entitites;

namespace Application.Interfaces.MappingProfiles;

public interface IMerchantMapper
{
    Merchant Map(CreateMerchantDto merchant, Guid userId);
}
