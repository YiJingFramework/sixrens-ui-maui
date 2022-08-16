using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.Paths;
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
            _ = builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    _ = fonts.AddFont("HongLeiBanShu.ttf", "HongLei");
                });

            RegisterServices(builder.Services);
            RegisterPages(builder.Services);

            return builder.Build();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            _ = services.AddLogging();
            _ = services.AddSingleton<ExceptionHandler>();

            _ = services.AddSingleton(FileSystem.Current);
            _ = services.AddSingleton<PathProvider>();

            _ = services.AddSingleton<SixRensCore>();
        }
        public static void RegisterPages(IServiceCollection services)
        {
            _ = services.AddSingleton<AppShell>();

            _ = services.AddSingleton<MainPage>();
            _ = services.AddSingleton<MainPageViewModel>();

            _ = services.AddSingleton<CaseCreationPage>();
            _ = services.AddSingleton<CaseCreationPageViewModel>();

            _ = services.AddSingleton<CaseManagementPage>();
            _ = services.AddSingleton<CaseManagementPageViewModel>();

            _ = services.AddSingleton<PluginManagementPage>();
            _ = services.AddSingleton<PluginManagementPageViewModel>();

            _ = services.AddSingleton<SettingsPage>();
            _ = services.AddSingleton<SettingsPageViewModel>();
        }
    }
}