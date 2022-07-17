namespace SixRens.UI.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		_ = builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
			    _ = fonts
					.AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
					.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        _ = builder.Services;

        return builder.Build();
	}
}
