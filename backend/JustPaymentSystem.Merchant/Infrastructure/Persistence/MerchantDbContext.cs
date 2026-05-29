using Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class MerchantDbContext : DbContext
{
    public MerchantDbContext(DbContextOptions<MerchantDbContext> options) : base(options) { }

    public DbSet<Merchant> Merchants => Set<Merchant>();
    public DbSet<BusinessType> BusinessTypes => Set<BusinessType>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<ApiCredential> ApiCredentials => Set<ApiCredential>();
    public DbSet<Webhook> Webhooks => Set<Webhook>();
    public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();
    public DbSet<EventType> EventTypes => Set<EventType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WebhookEvent>()
            .HasKey(we => new { we.WebhookId, we.EventId });

        modelBuilder.Entity<WebhookEvent>()
            .HasOne(we => we.Webhook)
            .WithMany(w => w.WebhookEvents)
            .HasForeignKey(we => we.WebhookId);

        modelBuilder.Entity<WebhookEvent>()
            .HasOne(we => we.Event)
            .WithMany(e => e.WebhookEvents)
            .HasForeignKey(we => we.EventId);
            
        modelBuilder.Entity<Merchant>()
            .HasOne(m => m.Location).WithOne(l => l.Merchant).HasForeignKey<Location>(l => l.MerchantId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Merchant>()
            .HasOne(m => m.Contact).WithOne(c => c.Merchant).HasForeignKey<Contact>(c => c.MerchantId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Merchant>()
            .HasOne(m => m.Webhook).WithOne(w => w.Merchant).HasForeignKey<Webhook>(w => w.MerchantId).OnDelete(DeleteBehavior.Cascade);
    }
}
