using MudBlazor.Services;
using Microsoft.Extensions.Logging;
using Personal_Expense_Tracking.ViewModels;
using Personal_Expense_Tracking.Services;

namespace Personal_Expense_Tracking
{
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

            // Add Blazor WebView
            builder.Services.AddMauiBlazorWebView();
            
            builder.Services.AddScoped<UserService>();




            // Add MudBlazor services
            builder.Services.AddMudServices();  // Registers MudBlazor services

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();  // Enable developer tools in Debug mode
            builder.Logging.AddDebug();  // Add debug logging
#endif

            return builder.Build();  // Build and return the Maui app
        }
    }
}
