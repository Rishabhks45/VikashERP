using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using MudBlazor.Services;
using VikashERP.Web.Validation;
using VikashERP.Web.Auth;
using VikashERP.Web.Components;
using VikashERP.Web.Services;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web;

public class WebUIStartup { }

public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        // Add Razor Components
        _ = builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(x => { x.DetailedErrors = true; })
            .AddHubOptions(x => { x.MaximumParallelInvocationsPerClient = 1; x.MaximumReceiveMessageSize = 1024 * 1024 * 5; });

        builder.Services.AddControllers();
        builder.Services.AddMudServices();
        builder.Services.AddHttpContextAccessor();

        var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7013";

        builder.Services.AddTransient<JwtTokenHandler>();

        void ConfigureApiClient(IHttpClientBuilder clientBuilder) =>
            clientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

        if (builder.Environment.IsDevelopment())
        {
            ConfigureApiClient(builder.Services.AddHttpClient("VikashERP.Api.Anonymous", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }));

            ConfigureApiClient(builder.Services.AddHttpClient("VikashERP.Api", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<JwtTokenHandler>());
        }
        else
        {
            builder.Services.AddHttpClient("VikashERP.Api.Anonymous", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            builder.Services.AddHttpClient("VikashERP.Api", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<JwtTokenHandler>();
        }

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "VikashERP.AuthCookie";
                options.LoginPath = "/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.SlidingExpiration = true;
            });

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        builder.Services.AddScoped<TokenValidator>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IDarkModeService, DarkModeService>();
        builder.Services.AddScoped<IOrganizationBrandingService, OrganizationBrandingService>();
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
        builder.Services.AddScoped<IUserProfileService, UserProfileService>();
        builder.Services.AddScoped<ICustomerShopService, CustomerShopService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductWebService, ProductWebService>();
        builder.Services.AddScoped<ISupplierWebService, SupplierWebService>();
        builder.Services.AddScoped<IBrokerWebService, BrokerWebService>();
        builder.Services.AddScoped<IPurchaseWebService, PurchaseWebService>();
        builder.Services.AddScoped<IInventoryWebService, InventoryWebService>();
        builder.Services.AddScoped<ISalesWebService, SalesWebService>();
        builder.Services.AddScoped<ICustomerWebService, CustomerWebService>();
        builder.Services.AddScoped<IExpenseWebService, ExpenseWebService>();
        builder.Services.AddScoped<IHolidayWebService, HolidayWebService>();
        builder.Services.AddScoped<ITimezoneService, TimezoneService>();
        builder.Services.AddValidatorsFromAssemblyContaining<LoginFormModelValidator>();
    }

    public static void UseServices(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}
