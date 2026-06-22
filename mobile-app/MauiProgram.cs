using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using MobileApp.Services;
using MobileApp.ViewModels;
using MobileApp.Views;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register HTTP client and services
        builder.Services.AddSingleton<HttpClient>(sp => new HttpClient 
        { 
            BaseAddress = new Uri(ApiConfig.BaseUrl) 
        });
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // Register ViewModels and Views
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
