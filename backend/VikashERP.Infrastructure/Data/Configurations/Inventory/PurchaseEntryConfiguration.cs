using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Inventory;

public class PurchaseEntryConfiguration : IEntityTypeConfiguration<PurchaseEntry>
{
    public void Configure(EntityTypeBuilder<PurchaseEntry> builder)
    {
        builder.ToTable("PurchaseEntries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntryNumber)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasIndex(x => x.EntryNumber).IsUnique();

        builder.Property(x => x.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.Property(x => x.VehicleNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Ignore(x => x.NetAmount);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.PurchaseEntry)
            .HasForeignKey(x => x.PurchaseEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
