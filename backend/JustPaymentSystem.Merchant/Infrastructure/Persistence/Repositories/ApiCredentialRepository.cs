using Application.Interfaces.Repositories;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ApiCredentialRepository : RepositoryBase<ApiCredential>, IApiCredentialRepository
{
    private readonly MerchantDbContext _context;
    public ApiCredentialRepository(MerchantDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<ApiCredential?> GetByMerchantIdAsync(Guid merchantId)
    {
        return await _context.ApiCredentials
            .Include(c => c.Merchant)
            .FirstOrDefaultAsync(c => c.Merchant != null && c.MerchantId == merchantId);
    }

    public async Task<ApiCredential?> GetByUserIdAsync(Guid userId)
    {
        return await _context.ApiCredentials
            .Include(c => c.Merchant)
            .FirstOrDefaultAsync(c => c.Merchant != null && c.Merchant.UserId == userId);
    }
    public async Task CreateApiCredentialAsync(ApiCredential apiCredential)
    {
        await _context.ApiCredentials.AddAsync(apiCredential);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}