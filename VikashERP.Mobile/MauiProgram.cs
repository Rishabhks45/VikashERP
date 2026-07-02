using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using VikashERP.Mobile.Services;
using VikashERP.Mobile.Services.Interfaces;
using Cropper.Blazor.Extensions;

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

		// Configure HttpClient for API
		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://1m59w80r-7013.inc1.devtunnels.ms/") });
		builder.Services.AddScoped<ApiClient>();
		builder.Services.AddScoped<VikashERP.Mobile.State.AppStateService>();

		builder.Services.AddScoped<IUserProfileService, UserProfileService>();
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
