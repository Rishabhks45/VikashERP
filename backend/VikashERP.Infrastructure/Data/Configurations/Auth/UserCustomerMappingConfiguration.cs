using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Auth;

public class UserCustomerMappingConfiguration : IEntityTypeConfiguration<UserCustomerMapping>
{
    public void Configure(EntityTypeBuilder<UserCustomerMapping> entity)
    {

            entity.ToTable("UserCustomerMappings");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasIndex(e => e.CustomerId);
            entity.HasOne(e => e.User)
                  .WithOne(u => u.CustomerMapping)
                  .HasForeignKey<UserCustomerMapping>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.UserMappings)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
