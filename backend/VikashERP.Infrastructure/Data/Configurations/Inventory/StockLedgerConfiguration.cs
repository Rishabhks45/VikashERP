using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Inventory;

public class StockLedgerConfiguration : IEntityTypeConfiguration<StockLedger>
{
    public void Configure(EntityTypeBuilder<StockLedger> entity)
    {

            entity.ToTable("StockLedgers");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.WeightKg).HasPrecision(12, 3);
            entity.Property(e => e.RunningWeightKg).HasPrecision(12, 3);
            entity.HasOne(e => e.Variant)
                  .WithMany(v => v.StockLedgerEntries)
                  .HasForeignKey(e => e.VariantId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Godown)
                  .WithMany(g => g.StockLedgerEntries)
                  .HasForeignKey(e => e.GodownId)
                  .OnDelete(DeleteBehavior.Restrict);
    }
}
