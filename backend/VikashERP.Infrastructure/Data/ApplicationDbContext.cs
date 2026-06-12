using Microsoft.EntityFrameworkCore;
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
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            entity.Property(e => e.Role)
                  .HasConversion(
                      role => role.ToFriendlyName(),
                      value => UserRoleExtensions.FromString(value) ?? UserRole.Customer)
                  .IsRequired()
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetTokens");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserCustomerMapping>(entity =>
        {
            entity.ToTable("UserCustomerMappings");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
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
            entity.ToTable("godowns");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).HasColumnName("location").HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            entity.Property(e => e.CompanyName).HasColumnName("company_name").HasMaxLength(255);
            entity.Property(e => e.Phone).HasColumnName("phone").IsRequired().HasMaxLength(20);
            entity.Property(e => e.Gstin).HasColumnName("gstin").HasMaxLength(15);
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CurrentBalance).HasColumnName("current_balance").HasPrecision(12, 2);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(e => e.Id);
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
            entity.Property(e => e.CreatedAt);
            entity.HasIndex(e => e.AccountNumber).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => e.Name).IsUnique();
        });
    }

    private static void ConfigureProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
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
            entity.ToTable("product_variants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Size).HasColumnName("size").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Thickness).HasColumnName("thickness").IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitPcsToKg).HasColumnName("unit_pcs_to_kg").HasPrecision(12, 4);
            entity.Property(e => e.AlertQtyPcs).HasColumnName("alert_qty_pcs");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => new { e.ProductId, e.Size, e.Thickness }).IsUnique();
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
            entity.ToTable("stock_ledger");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");
            entity.Property(e => e.GodownId).HasColumnName("godown_id");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type").IsRequired().HasMaxLength(50);
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.QtyPcs).HasColumnName("qty_pcs");
            entity.Property(e => e.WeightKg).HasColumnName("weight_kg").HasPrecision(12, 3);
            entity.Property(e => e.RunningPcs).HasColumnName("running_pcs");
            entity.Property(e => e.RunningWeightKg).HasColumnName("running_weight_kg").HasPrecision(12, 3);
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
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
            entity.ToTable("invoices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal").HasPrecision(12, 2);
            entity.Property(e => e.CgstAmount).HasColumnName("cgst_amount").HasPrecision(12, 2);
            entity.Property(e => e.SgstAmount).HasColumnName("sgst_amount").HasPrecision(12, 2);
            entity.Property(e => e.IgstAmount).HasColumnName("igst_amount").HasPrecision(12, 2);
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasPrecision(12, 2);
            entity.Property(e => e.PaidAmount).HasColumnName("paid_amount").HasPrecision(12, 2);
            entity.Property(e => e.DueAmount).HasColumnName("due_amount").HasPrecision(12, 2);
            entity.Property(e => e.PaymentMode).HasColumnName("payment_mode").IsRequired().HasMaxLength(50);
            entity.Property(e => e.InvoiceDate).HasColumnName("invoice_date");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.ToTable("invoice_items");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");
            entity.Property(e => e.QtyPcs).HasColumnName("qty_pcs");
            entity.Property(e => e.WeightKg).HasColumnName("weight_kg").HasPrecision(12, 3);
            entity.Property(e => e.RatePerKg).HasColumnName("rate_per_kg").HasPrecision(12, 2);
            entity.Property(e => e.CgstRate).HasColumnName("cgst_rate").HasPrecision(5, 2);
            entity.Property(e => e.SgstRate).HasColumnName("sgst_rate").HasPrecision(5, 2);
            entity.Property(e => e.IgstRate).HasColumnName("igst_rate").HasPrecision(5, 2);
            entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasPrecision(12, 2);
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
            entity.ToTable("customer_ledger");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type").IsRequired().HasMaxLength(50);
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.Debit).HasColumnName("debit").HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasColumnName("credit").HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasColumnName("running_balance").HasPrecision(12, 2);
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.LedgerEntries)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SupplierLedger>(entity =>
        {
            entity.ToTable("supplier_ledger");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type").IsRequired().HasMaxLength(50);
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.Debit).HasColumnName("debit").HasPrecision(12, 2);
            entity.Property(e => e.Credit).HasColumnName("credit").HasPrecision(12, 2);
            entity.Property(e => e.RunningBalance).HasColumnName("running_balance").HasPrecision(12, 2);
            entity.Property(e => e.Remarks).HasColumnName("remarks");
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
            entity.ToTable("deliveries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.VehicleNumber).HasColumnName("vehicle_number").IsRequired().HasMaxLength(20);
            entity.Property(e => e.DriverName).HasColumnName("driver_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.DriverPhone).HasColumnName("driver_phone").HasMaxLength(20);
            entity.Property(e => e.DeliveryStatus).HasColumnName("delivery_status").IsRequired().HasMaxLength(50);
            entity.Property(e => e.DeliveryChallanNumber).HasColumnName("delivery_challan_number").HasMaxLength(100);
            entity.Property(e => e.LoadingCharge).HasColumnName("loading_charge").HasPrecision(10, 2);
            entity.Property(e => e.FreightCharge).HasColumnName("freight_charge").HasPrecision(10, 2);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => e.DeliveryChallanNumber).IsUnique();
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
            entity.ToTable("staff");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).HasColumnName("role").IsRequired().HasMaxLength(50);
            entity.Property(e => e.SalaryPerMonth).HasColumnName("salary_per_month").HasPrecision(10, 2);
            entity.Property(e => e.Phone).HasColumnName("phone").IsRequired().HasMaxLength(20);
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.HasIndex(e => e.Phone).IsUnique();
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("attendance");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.WorkDate).HasColumnName("work_date");
            entity.Property(e => e.Status).HasColumnName("status").IsRequired().HasMaxLength(20);
            entity.Property(e => e.CheckIn).HasColumnName("check_in");
            entity.Property(e => e.CheckOut).HasColumnName("check_out");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => new { e.StaffId, e.WorkDate }).IsUnique();
            entity.HasOne(e => e.Staff)
                  .WithMany(s => s.AttendanceRecords)
                  .HasForeignKey(e => e.StaffId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StaffSalary>(entity =>
        {
            entity.ToTable("staff_salaries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.AmountPaid).HasColumnName("amount_paid").HasPrecision(10, 2);
            entity.Property(e => e.PaymentMode).HasColumnName("payment_mode").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Remarks).HasColumnName("remarks");
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
            entity.HasIndex(e => new { e.TemplateKey, e.NotificationType }).IsUnique();
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
