using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Brokers;

public class BrokerLedgerConfiguration : IEntityTypeConfiguration<BrokerLedger>
{
    public void Configure(EntityTypeBuilder<BrokerLedger> builder)
    {
        builder.ToTable("BrokerLedgers");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.TransactionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Debit).HasPrecision(12, 2).IsRequired();
        builder.Property(e => e.Credit).HasPrecision(12, 2).IsRequired();
        builder.Property(e => e.RunningBalance).HasPrecision(12, 2).IsRequired();

        builder.HasOne(e => e.Broker)
            .WithMany()
            .HasForeignKey(e => e.BrokerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
