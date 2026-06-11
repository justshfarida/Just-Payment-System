using Application.Interfaces.Repositories;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BusinessTypeRepository : RepositoryBase<BusinessType>, IBusinessTypeRepository
{
    private readonly MerchantDbContext _context;

    public BusinessTypeRepository(MerchantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BusinessType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BusinessTypes
            .AsNoTracking()
            .OrderBy(businessType => businessType.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<BusinessType?> GetByIdAsync(Guid id, bool trackChanges = false)
    {
        var query = _context.BusinessTypes.Where(businessType => businessType.Id == id);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _context.BusinessTypes
            .AsNoTracking()
            .AnyAsync(businessType => businessType.Name == name, cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default)
    {
        return _context.BusinessTypes
            .AsNoTracking()
            .AnyAsync(businessType => businessType.Name == name && businessType.Id != excludeId, cancellationToken);
    }

    public Task<bool> HasMerchantsAsync(Guid businessTypeId, CancellationToken cancellationToken = default)
    {
        return _context.Merchants
            .AsNoTracking()
            .AnyAsync(merchant => merchant.TypeId == businessTypeId, cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
