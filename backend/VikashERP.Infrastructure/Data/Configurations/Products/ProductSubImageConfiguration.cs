using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Products;

public class ProductSubImageConfiguration : IEntityTypeConfiguration<ProductSubImage>
{
    public void Configure(EntityTypeBuilder<ProductSubImage> entity)
    {

            entity.ToTable("ProductSubImages");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => new { e.ProductId, e.DisplayOrder });
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.SubImages)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
