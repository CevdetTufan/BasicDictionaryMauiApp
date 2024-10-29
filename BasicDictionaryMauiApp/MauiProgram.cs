﻿using BasicDictionary.Dal.MongoDb;
using BasicDictionary.Dal.Repositories;
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

			builder.Services.AddSingleton(
				new MongoDbSettings(
					connectionString: "your-connection-string", 
					databaseName: "your-database-name"));

			builder.Services.AddSingleton(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

			builder.Services.AddSingleton<MainPageViewModel>();
			builder.Services.AddSingleton<MainPage>();

			builder.Services.AddSingleton<WordAddPageViewModel>();
			builder.Services.AddSingleton<WordAddPage>();

			builder.Services.AddSingleton<WordListViewModel>();
			builder.Services.AddSingleton<WordListPage>();

			builder.Services.AddSingleton<IWordService, WordServiceJson>();
			builder.Services.AddSingleton<IDeletedWordLogger, DeletedWordLoggerJson>();

			return builder.Build();
		}
	}
}
