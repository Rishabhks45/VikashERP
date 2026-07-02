using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Timezones;

public class TimezoneConfiguration : IEntityTypeConfiguration<Timezone>
{
    public void Configure(EntityTypeBuilder<Timezone> entity)
    {
        entity.ToTable("timezones");
        
        entity.Property(e => e.Id).HasColumnName("timezone_id");
        
        entity.HasIndex(e => e.IanaId)
            .IsUnique()
            .HasFilter("\"is_deleted\" = false");
    }
}
