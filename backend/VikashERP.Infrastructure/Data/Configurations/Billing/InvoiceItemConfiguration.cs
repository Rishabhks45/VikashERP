using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Billing;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> entity)
    {

            entity.ToTable("InvoiceItems");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.WeightKg).HasPrecision(12, 3);
            entity.Property(e => e.Rate).HasPrecision(12, 2);
            entity.Property(e => e.RateOn).HasConversion<int>().IsRequired();
            entity.Property(e => e.CgstRate).HasPrecision(5, 2);
            entity.Property(e => e.SgstRate).HasPrecision(5, 2);
            entity.Property(e => e.IgstRate).HasPrecision(5, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(12, 2);
            entity.HasOne(e => e.Invoice)
                  .WithMany(i => i.Items)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Variant)
                  .WithMany(v => v.InvoiceItems)
                  .HasForeignKey(e => e.VariantId)
                  .OnDelete(DeleteBehavior.Restrict);
    }
}
