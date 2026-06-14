using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Masters;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {

            entity.ToTable("Customers");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Gstin).HasMaxLength(15);
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.DefaultPaymentMode)
                  .HasConversion(
                      mode => mode.ToStorageValue(),
                      value => CustomerPaymentModeExtensions.FromString(value) ?? CustomerPaymentMode.Account)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(e => e.CreditLimit).HasPrecision(12, 2);
            entity.Property(e => e.CurrentBalance).HasPrecision(12, 2);
            entity.Property(e => e.DefaultFreightRate).HasPrecision(12, 2);
            entity.HasIndex(e => e.AccountNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}
