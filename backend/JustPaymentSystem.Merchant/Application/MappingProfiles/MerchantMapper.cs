using Application.Dtos;
using Application.Interfaces.MappingProfiles;
using Domain.Entitites;
using Domain.Enums;

namespace Application.MappingProfiles;


public class MerchantMapper: IMerchantMapper
{
    public Merchant Map(CreateMerchantDto merchant, Guid userId)
    {
        return new Merchant
        {
            Name = merchant.Name,
            VOEN = merchant.VOEN,
            UserId = userId,
            TypeId = merchant.TypeId,
            Location = new Location
            {
                Country = merchant.Country,
                City = merchant.City,
                Address = merchant.Address
            },
            Contact = new Contact
            {
                Email = merchant.Email,
                Phone = merchant.PhoneNumber
            },
            Status = Status.Pending
        };
    }
}