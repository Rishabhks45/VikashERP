using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Auth;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> entity)
    {

            entity.ToTable("PasswordResetTokens");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
    }
}
