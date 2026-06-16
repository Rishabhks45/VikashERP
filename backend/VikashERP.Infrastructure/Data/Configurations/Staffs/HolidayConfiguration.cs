using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Staffs;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> entity)
    {
        entity.ToTable("Holidays");
        entity.HasKey(e => e.Id);
        entity.ConfigureGuidPrimaryKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.Date).IsRequired();
        entity.Property(e => e.IsRecurring).IsRequired().HasDefaultValue(false);
    }
}
