using Domain.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(x => x.PaymentSnapshot)
         .WithOne(x => x.Transaction)
         .HasForeignKey<PaymentSnapshot>(x => x.TransactionId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.Currency)
            .HasMaxLength(3)
            .IsFixedLength();

    }
}
