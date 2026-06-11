using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Interfaces;

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
        services.AddScoped<IEmailTemplateRepository, Repositories.EmailTemplateRepository>();
        services.AddScoped<IEmailTemplateService, Email.EmailTemplateService>();
        services.AddScoped<ISharedRepository, Repositories.SharedRepository>();

        return services;
    }
}
