using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Interfaces;
using VikashERP.Infrastructure.Brokers;

namespace VikashERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Data.ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repositories.Repository<>));
        services.AddScoped<IUnitOfWork, Repositories.UnitOfWork>();
        services.AddScoped<IUserRepository, Repositories.UserRepository>();
        services.AddScoped<ICustomerRepository, Repositories.CustomerRepository>();
        services.AddScoped<IUserCustomerMappingRepository, Repositories.UserCustomerMappingRepository>();
        services.AddScoped<IEmailTemplateRepository, Repositories.EmailTemplateRepository>();
        services.AddScoped<IEmailTemplateService, Email.EmailTemplateService>();
        services.AddScoped<IOrganizationRepository, Repositories.OrganizationRepository>();
        services.AddScoped<IOrganizationService, Services.OrganizationService>();
        services.AddScoped<ICategoryRepository, Repositories.CategoryRepository>();
        services.AddScoped<ICategoryService, Categories.CategoryService>();
        services.AddScoped<IProductRepository, Repositories.ProductRepository>();
        services.AddScoped<IProductService, Products.ProductService>();
        services.AddScoped<ISupplierRepository, Suppliers.SupplierRepository>();
        services.AddScoped<ISupplierService, Suppliers.SupplierService>();
        services.AddScoped<IPurchaseRepository, Purchases.PurchaseRepository>();
        services.AddScoped<IPurchaseService, Purchases.PurchaseService>();
        services.AddScoped<IBrokerRepository, Brokers.BrokerRepository>();
        services.AddScoped<IBrokerService, BrokerService>();
        services.AddScoped<IInventoryService, Inventory.InventoryService>();
        services.AddScoped<VikashERP.Application.Interfaces.ISalesRepository, Sales.SalesRepository>();
        services.AddScoped<VikashERP.Application.Interfaces.ISalesService, Sales.SalesService>();
        services.AddScoped<IExpenseRepository, Expenses.ExpenseRepository>();
        services.AddScoped<IExpenseService, Expenses.ExpenseService>();
        services.AddScoped<ISharedRepository, Repositories.SharedRepository>();
        services.AddScoped<IHolidayService, Holidays.HolidayService>();

        return services;
    }
}
