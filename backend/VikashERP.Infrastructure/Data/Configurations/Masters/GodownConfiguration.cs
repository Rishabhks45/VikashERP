using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data.Configurations.Masters;

public class GodownConfiguration : IEntityTypeConfiguration<Godown>
{
    public void Configure(EntityTypeBuilder<Godown> entity)
    {

            entity.ToTable("Godowns");
            entity.HasKey(e => e.Id);
            entity.ConfigureGuidPrimaryKey( e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.HasIndex(e => e.Name).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}
