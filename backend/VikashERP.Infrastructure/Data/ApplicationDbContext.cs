using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;
using VikashERP.Infrastructure.Data.Configurations.Brokers;
using VikashERP.Application.Interfaces;

namespace VikashERP.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService? currentUserService = null) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserCustomerMapping> UserCustomerMappings => Set<UserCustomerMapping>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<Godown> Godowns => Set<Godown>();
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierLedger> SupplierLedgers { get; set; }
    public DbSet<Broker> Brokers { get; set; }
    public DbSet<BrokerLedger> BrokerLedgers { get; set; }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSubImage> ProductSubImages => Set<ProductSubImage>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<StockLedger> StockLedgers => Set<StockLedger>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<CustomerLedger> CustomerLedgers => Set<CustomerLedger>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<StaffSalary> StaffSalaries => Set<StaffSalary>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<PurchaseEntry> PurchaseEntries => Set<PurchaseEntry>();
    public DbSet<PurchaseEntryItem> PurchaseEntryItems => Set<PurchaseEntryItem>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<Timezone> Timezones => Set<Timezone>();
    public DbSet<SalaryConfiguration> SalaryConfigurations => Set<SalaryConfiguration>();

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var currentUserId = _currentUserService?.UserId;
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy ??= currentUserId;
                    entry.Entity.IsActive = true;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy ??= currentUserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.IsActive = false;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy ??= currentUserId;
                    break;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
