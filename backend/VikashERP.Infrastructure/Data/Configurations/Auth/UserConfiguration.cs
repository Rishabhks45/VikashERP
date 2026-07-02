using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Auth;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {

            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.Property(e => e.ProfilePictureUrl);
            entity.Property(e => e.Role)
                  .HasConversion(
                      role => role.ToFriendlyName(),
                      value => UserRoleExtensions.FromString(value) ?? UserRole.Customer)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);
            entity.Property(e => e.LastLoginAt);
    }
}
