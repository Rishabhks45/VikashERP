using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Staffs;

public class StaffSalaryConfiguration : IEntityTypeConfiguration<StaffSalary>
{
    public void Configure(EntityTypeBuilder<StaffSalary> entity)
    {

            entity.ToTable("StaffSalaries");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
            entity.Property(e => e.PaymentMode).IsRequired().HasMaxLength(50);
            entity.HasOne(e => e.Staff)
                  .WithMany(s => s.SalaryPayments)
                  .HasForeignKey(e => e.StaffId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
