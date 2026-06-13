using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Ledgers;

public class CustomerLedgerConfiguration : IEntityTypeConfiguration<CustomerLedger>
{
    public void Configure(EntityTypeBuilder<CustomerLedger> entity)
    {

            entity.ToTable("CustomerLedgers");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Debit).HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasPrecision(12, 2);
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.LedgerEntries)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
