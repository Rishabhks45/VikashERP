using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Deliveries;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> entity)
    {

            entity.ToTable("Deliveries");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.VehicleNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DriverName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DriverPhone).HasMaxLength(20);
            entity.Property(e => e.DeliveryStatus).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DeliveryChallanNumber).HasMaxLength(100);
            entity.Property(e => e.LoadingCharge).HasPrecision(10, 2);
            entity.Property(e => e.FreightCharge).HasPrecision(10, 2);
            entity.HasIndex(e => e.DeliveryChallanNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Invoice)
                  .WithMany(i => i.Deliveries)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
