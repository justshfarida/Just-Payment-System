using Application.Interfaces;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MerchantRepository : RepositoryBase<Merchant>, IMerchantRepository
{
    private readonly MerchantDbContext _context;

    public MerchantRepository(MerchantDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Merchant?> GetWebhookSettingsAsync(Guid merchantId, string eventType)
    {
        // Fetch the merchant along with their active webhook details and specific event subscriptions
        return await _context.Merchants
            .Include(m => m.Webhook)
                .ThenInclude(w => w!.WebhookEvents!)
                    .ThenInclude(we => we.Event)
            .FirstOrDefaultAsync(m => m.Id == merchantId && m.Webhook != null && m.Webhook.IsActive);
    }

    public async Task AddMerchantAsync(Merchant merchant)
    {
        await _context.Merchants.AddAsync(merchant);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
