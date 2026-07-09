using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using VikashERP.Mobile.Services;
using VikashERP.Mobile.Services.Interfaces;
using Cropper.Blazor.Extensions;
using Microsoft.Maui.LifecycleEvents;

namespace VikashERP.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});


		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddMudServices();

		// Register Delegating Handler
		builder.Services.AddTransient<JwtDelegatingHandler>();

		// Configure HttpClient for Auth (No Interceptor)
		builder.Services.AddHttpClient("AuthClient", client => 
		{
			// Render server
			//client.BaseAddress = new Uri("https://vikasherp-api.onrender.com/");
			// local server
			//client.BaseAddress = new Uri("https://1m59w80r-7013.inc1.devtunnels.ms/");
			// MonsterAsp Server
			client.BaseAddress = new Uri("https://vikashironapi.runasp.net/");
		});

		// Configure HttpClient for API (With Interceptor)
		builder.Services.AddHttpClient("ApiClient", client => 
		{
			// Render server
			//client.BaseAddress = new Uri("https://vikasherp-api.onrender.com/");
			// local server
			//client.BaseAddress = new Uri("https://1m59w80r-7013.inc1.devtunnels.ms/");
			// MonsterAsp Server
			client.BaseAddress = new Uri("https://vikashironapi.runasp.net/");
		})
		.AddHttpMessageHandler<JwtDelegatingHandler>();

		builder.Services.AddScoped<ApiClient>();
		builder.Services.AddScoped<VikashERP.Mobile.State.AppStateService>();

		builder.Services.AddScoped<IUserProfileService, UserProfileService>();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<ICustomerService, CustomerService>();
		builder.Services.AddScoped<IInventoryService, InventoryService>();
		builder.Services.AddScoped<ISalesService, SalesService>();
		builder.Services.AddScoped<IOrganizationService, OrganizationService>();
		builder.Services.AddScoped<IPdfExportService, PdfExportService>();
		builder.Services.AddScoped<IHolidayService, HolidayService>();
		builder.Services.AddScoped<IBrokerService, BrokerService>();
		builder.Services.AddScoped<IExpenseService, ExpenseService>();
		builder.Services.AddScoped<ISalaryConfigurationService, SalaryConfigurationService>();
		builder.Services.AddScoped<IAuthService, AuthService>();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, VikashERP.Mobile.State.CustomAuthStateProvider>();
		
		// Register Cropper.Blazor services
		builder.Services.AddCropper();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
