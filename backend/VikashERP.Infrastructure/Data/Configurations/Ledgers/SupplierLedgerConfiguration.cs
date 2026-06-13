using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Ledgers;

public class SupplierLedgerConfiguration : IEntityTypeConfiguration<SupplierLedger>
{
    public void Configure(EntityTypeBuilder<SupplierLedger> entity)
    {

            entity.ToTable("SupplierLedgers");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Debit).HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasPrecision(12, 2);
            entity.HasOne(e => e.Supplier)
                  .WithMany(s => s.LedgerEntries)
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
