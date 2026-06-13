using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Billing;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> entity)
    {

            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Subtotal).HasPrecision(12, 2);
            entity.Property(e => e.CgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.SgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.IgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(12, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(12, 2);
            entity.Property(e => e.DueAmount).HasPrecision(12, 2);
            entity.Property(e => e.PaymentMode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
    }
}
