using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Brokers;

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable("Brokers");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.CurrentBalance)
            .HasPrecision(12, 2)
            .IsRequired();
            
        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}
