using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Inventory;

public class PurchaseEntryItemConfiguration : IEntityTypeConfiguration<PurchaseEntryItem>
{
    public void Configure(EntityTypeBuilder<PurchaseEntryItem> builder)
    {
        builder.ToTable("PurchaseEntryItems");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ProductVariant)
            .WithMany()
            .HasForeignKey(x => x.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
