using Domain.Domains;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionAttribute> TransactionAttributes { get; set; }
    public DbSet<PaymentSnapshot> PaymentSnapshots { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
    }
}
