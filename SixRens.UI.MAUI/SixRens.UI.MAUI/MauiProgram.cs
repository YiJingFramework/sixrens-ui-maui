using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.ViewModels;
using SixRens.UI.MAUI.Views;

namespace SixRens.UI.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RegisterServices(builder.Services);
            RegisterPages(builder.Services);
        
            return builder.Build();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<ExceptionHandler>();
            services.AddSingleton<SixRensCore>();
        }
        public static void RegisterPages(IServiceCollection services)
        {
            services.AddSingleton<AppShell>();

            services.AddSingleton<MainPage>();
            services.AddSingleton<MainPageViewModel>();
        }
    }
}