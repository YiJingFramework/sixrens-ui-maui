using CommunityToolkit.Maui;
using SixRens.UI.MAUI.Pages;
using SixRens.UI.MAUI.Pages.CaseCreation;
using SixRens.UI.MAUI.Pages.CaseManagement;
using SixRens.UI.MAUI.Pages.Main;
using SixRens.UI.MAUI.Pages.PluginManagement;
using SixRens.UI.MAUI.Pages.Settings;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.Paths;
using SixRens.UI.MAUI.Services.Preferring;
using SixRens.UI.MAUI.Services.SixRens;

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
                })
                .UseMauiCommunityToolkit()
                ;

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

            _ = services.AddSingleton(Preferences.Default);
            _ = services.AddSingleton<PreferenceManager>();

            _ = services.AddSingleton(FilePicker.Default);

            _ = services.AddSingleton<SixRensCore>();
        }
        public static void RegisterPages(IServiceCollection services)
        {
            _ = services.AddSingleton<AppShell>();

            _ = services.AddSingleton<MainPage>();
            _ = services.AddSingleton<CaseCreationPage>();
            _ = services.AddSingleton<CaseManagementPage>();
            _ = services.AddSingleton<PluginManagementPage>();
            _ = services.AddSingleton<SettingsPage>();
        }
    }
}