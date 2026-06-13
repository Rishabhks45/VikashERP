using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Products;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> entity)
    {

            entity.ToTable("ProductVariants");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.Size).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Thickness).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitPcsToKg).HasPrecision(12, 4);
            entity.HasIndex(e => new { e.ProductId, e.Size, e.Thickness }).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
