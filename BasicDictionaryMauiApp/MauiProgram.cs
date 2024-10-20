using BasicDictionaryMauiApp.Pages;
using BasicDictionaryMauiApp.Services;
using BasicDictionaryMauiApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace BasicDictionaryMauiApp
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
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Services.AddSingleton<IServiceProvider, ServiceProvider>();

			builder.Services.AddSingleton<WordAddPageViewModel>();
			builder.Services.AddSingleton<WordAddPage>();

			builder.Services.AddSingleton<WordListViewModel>();
			builder.Services.AddSingleton<WordListPage>();
			
			builder.Services.AddSingleton<IWordService, WordServiceJson>();
			

			return builder.Build();
		}
	}
}
