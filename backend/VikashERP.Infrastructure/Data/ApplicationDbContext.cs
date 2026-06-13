using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserCustomerMapping> UserCustomerMappings => Set<UserCustomerMapping>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<Godown> Godowns => Set<Godown>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSubImage> ProductSubImages => Set<ProductSubImage>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<StockLedger> StockLedgers => Set<StockLedger>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<CustomerLedger> CustomerLedgers => Set<CustomerLedger>();
    public DbSet<SupplierLedger> SupplierLedgers => Set<SupplierLedger>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<StaffSalary> StaffSalaries => Set<StaffSalary>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Organization> Organizations => Set<Organization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyErpSchema();
    }
}

internal static class ErpSchemaConfiguration
{
    private static void ConfigureGuidPrimaryKey<TEntity>(
        EntityTypeBuilder<TEntity> entity,
        Expression<Func<TEntity, Guid>> keyExpression) where TEntity : class =>
        entity.Property(keyExpression).ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");

    public static void ApplyErpSchema(this ModelBuilder modelBuilder)
    {
        ConfigureAuth(modelBuilder);
        ConfigureMasters(modelBuilder);
        ConfigureProducts(modelBuilder);
        ConfigureInventory(modelBuilder);
        ConfigureBilling(modelBuilder);
        ConfigureLedgers(modelBuilder);
        ConfigureDelivery(modelBuilder);
        ConfigureStaff(modelBuilder);
        ConfigureEmailTemplates(modelBuilder);
        ConfigureOrganization(modelBuilder);
    }

    private static void ConfigureAuth(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            entity.Property(e => e.Role)
                  .HasConversion(
                      role => role.ToFriendlyName(),
                      value => UserRoleExtensions.FromString(value) ?? UserRole.Customer)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);
            entity.Property(e => e.LastLoginAt);
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetTokens");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserCustomerMapping>(entity =>
        {
            entity.ToTable("UserCustomerMappings");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
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
        });
    }

    private static void ConfigureMasters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Godown>(entity =>
        {
            entity.ToTable("Godowns");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.HasIndex(e => e.Name).IsUnique().HasFilter("\"IsDeleted\" = false");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Gstin).HasMaxLength(15);
            entity.Property(e => e.CurrentBalance).HasPrecision(12, 2);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
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
            entity.HasIndex(e => e.AccountNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique().HasFilter("\"IsDeleted\" = false");
        });
    }

    private static void ConfigureProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.HsnCode).HasMaxLength(10);
            entity.Property(e => e.ProductImageUrl).HasMaxLength(500);
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductSubImage>(entity =>
        {
            entity.ToTable("ProductSubImages");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => new { e.ProductId, e.DisplayOrder });
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.SubImages)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.ToTable("ProductVariants");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Size).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Thickness).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitPcsToKg).HasPrecision(12, 4);
            entity.HasIndex(e => new { e.ProductId, e.Size, e.Thickness }).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureInventory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockLedger>(entity =>
        {
            entity.ToTable("StockLedgers");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.WeightKg).HasPrecision(12, 3);
            entity.Property(e => e.RunningWeightKg).HasPrecision(12, 3);
            entity.HasOne(e => e.Variant)
                  .WithMany(v => v.StockLedgerEntries)
                  .HasForeignKey(e => e.VariantId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Godown)
                  .WithMany(g => g.StockLedgerEntries)
                  .HasForeignKey(e => e.GodownId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureBilling(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Subtotal).HasPrecision(12, 2);
            entity.Property(e => e.CgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.SgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.IgstAmount).HasPrecision(12, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(12, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(12, 2);
            entity.Property(e => e.DueAmount).HasPrecision(12, 2);
            entity.Property(e => e.PaymentMode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.ToTable("InvoiceItems");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.WeightKg).HasPrecision(12, 3);
            entity.Property(e => e.RatePerKg).HasPrecision(12, 2);
            entity.Property(e => e.CgstRate).HasPrecision(5, 2);
            entity.Property(e => e.SgstRate).HasPrecision(5, 2);
            entity.Property(e => e.IgstRate).HasPrecision(5, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(12, 2);
            entity.HasOne(e => e.Invoice)
                  .WithMany(i => i.Items)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Variant)
                  .WithMany(v => v.InvoiceItems)
                  .HasForeignKey(e => e.VariantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureLedgers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerLedger>(entity =>
        {
            entity.ToTable("CustomerLedgers");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Debit).HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasPrecision(12, 2);
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.LedgerEntries)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SupplierLedger>(entity =>
        {
            entity.ToTable("SupplierLedgers");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Debit).HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasPrecision(12, 2);
            entity.HasOne(e => e.Supplier)
                  .WithMany(s => s.LedgerEntries)
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureDelivery(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Deliveries");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.VehicleNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DriverName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DriverPhone).HasMaxLength(20);
            entity.Property(e => e.DeliveryStatus).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DeliveryChallanNumber).HasMaxLength(100);
            entity.Property(e => e.LoadingCharge).HasPrecision(10, 2);
            entity.Property(e => e.FreightCharge).HasPrecision(10, 2);
            entity.HasIndex(e => e.DeliveryChallanNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Invoice)
                  .WithMany(i => i.Deliveries)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureStaff(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.ToTable("Staff");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SalaryPerMonth).HasPrecision(10, 2);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Phone).IsUnique().HasFilter("\"IsDeleted\" = false");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("Attendances");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => new { e.StaffId, e.WorkDate }).IsUnique().HasFilter("\"IsDeleted\" = false");
            entity.HasOne(e => e.Staff)
                  .WithMany(s => s.AttendanceRecords)
                  .HasForeignKey(e => e.StaffId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StaffSalary>(entity =>
        {
            entity.ToTable("StaffSalaries");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.AmountPaid).HasPrecision(10, 2);
            entity.Property(e => e.PaymentMode).IsRequired().HasMaxLength(50);
            entity.HasOne(e => e.Staff)
                  .WithMany(s => s.SalaryPayments)
                  .HasForeignKey(e => e.StaffId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureEmailTemplates(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.ToTable("email_templates");
            entity.HasKey(e => e.Id);
            ConfigureGuidPrimaryKey(entity, e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TemplateKey).HasColumnName("template_key").IsRequired().HasMaxLength(50);
            entity.Property(e => e.NotificationType).HasColumnName("notification_type").IsRequired();
            entity.Property(e => e.DisplayName).HasColumnName("display_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasMaxLength(500);
            entity.Property(e => e.Subject).HasColumnName("subject").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Headline).HasColumnName("headline").IsRequired().HasMaxLength(255);
            entity.Property(e => e.BodyHtml).HasColumnName("body_html").IsRequired();
            entity.Property(e => e.Preheader).HasColumnName("preheader").HasMaxLength(255);
            entity.Property(e => e.ButtonLabel).HasColumnName("button_label").HasMaxLength(100);
            entity.Property(e => e.ButtonLinkToken).HasColumnName("button_link_token").HasMaxLength(100);
            entity.Property(e => e.AvailableTokens).HasColumnName("available_tokens").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => new { e.TemplateKey, e.NotificationType }).IsUnique().HasFilter("\"IsDeleted\" = false");
        });
    }

    private static void ConfigureOrganization(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organizations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LegalName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Tagline).HasMaxLength(500);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.FaviconUrl).HasMaxLength(500);
            entity.Property(e => e.LoginBackgroundUrl).HasMaxLength(500);
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);
            entity.Property(e => e.AddressLine1).HasMaxLength(255);
            entity.Property(e => e.AddressLine2).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.PinCode).HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.WebsiteUrl).HasMaxLength(500);
            entity.Property(e => e.WhatsAppNumber).HasMaxLength(30);
            entity.Property(e => e.Gstin).HasMaxLength(15);
            entity.Property(e => e.Pan).HasMaxLength(10);
            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.BankAccountName).HasMaxLength(255);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.IfscCode).HasMaxLength(20);
            entity.Property(e => e.EmailFromName).HasMaxLength(255);
            entity.Property(e => e.EmailFromAddress).HasMaxLength(255);
            entity.Property(e => e.MetaTitle).HasMaxLength(255);
            entity.Property(e => e.MetaDescription).HasMaxLength(500);
            entity.Property(e => e.MetaKeywords).HasMaxLength(500);
            entity.Property(e => e.FooterText).HasMaxLength(1000);
            entity.Property(e => e.CopyrightText).HasMaxLength(500);
            entity.Property(e => e.SocialFacebookUrl).HasMaxLength(500);
            entity.Property(e => e.SocialInstagramUrl).HasMaxLength(500);
            entity.Property(e => e.SocialLinkedInUrl).HasMaxLength(500);
            entity.Property(e => e.SocialYoutubeUrl).HasMaxLength(500);
            entity.Property(e => e.DefaultCurrency).IsRequired().HasMaxLength(10);
            entity.Property(e => e.DefaultWeightUnit).IsRequired().HasMaxLength(10);
            entity.Property(e => e.TimeZone).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateFormat).IsRequired().HasMaxLength(30);
        });
    }
}
