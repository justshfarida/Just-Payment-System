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

    public async Task<ApiCredential?> GetByMerchantIdAsync(Guid merchantId, bool trackChanges = false)
    {
        var query = _context.ApiCredentials
            .Include(c => c.Merchant)
            .Where(c => c.MerchantId == merchantId);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<ApiCredential?> GetByUserIdAsync(Guid userId, bool trackChanges = false)
    {
        var query = _context.ApiCredentials
            .Include(c => c.Merchant)
            .Where(c => c.Merchant != null && c.Merchant.UserId == userId);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
