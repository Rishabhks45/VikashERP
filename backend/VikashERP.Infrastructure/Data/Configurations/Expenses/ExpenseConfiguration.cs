using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Data.Configurations.Expenses;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> entity)
    {
        entity.ToTable("Expenses");
        entity.HasKey(e => e.Id);
        entity.ConfigureGuidPrimaryKey(e => e.Id);
        
        entity.Property(e => e.ExpenseDate)
              .IsRequired();
              
        entity.Property(e => e.Category)
              .IsRequired()
              .HasMaxLength(100);
              
        entity.Property(e => e.Amount)
              .HasPrecision(12, 2)
              .IsRequired();
              
        entity.Property(e => e.PaymentMode)
              .IsRequired()
              .HasMaxLength(50);
              
        entity.Property(e => e.Remarks)
              .HasMaxLength(1000);
    }
}
