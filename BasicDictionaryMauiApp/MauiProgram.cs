using BasicDictionary.Dal.MongoDb;
using BasicDictionary.Dal.Repositories;
using BasicDictionaryMauiApp.Models;
using BasicDictionaryMauiApp.Models.Entities;
using BasicDictionaryMauiApp.Pages;
using BasicDictionaryMauiApp.Services;
using BasicDictionaryMauiApp.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

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

			var config = new ConfigurationBuilder()
					.AddUserSecrets<MongoDbSecretModel>()
					.Build();

			builder.Services.AddSingleton(
				new MongoDbSettings(
					connectionString: config["MongoDb:ConnectionString"],
					databaseName: config["MongoDb:DatabaseName"]));

			builder.Services.AddSingleton<MongoDbContext>();
			builder.Services.AddSingleton(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

			builder.Services.AddSingleton(sp =>
			{
				var settings = sp.GetRequiredService<MongoDbSettings>();

				var client = new MongoClient(settings.ConnectionString);
				var database = client.GetDatabase(settings.DatabaseName);
				return database.GetCollection<WordModel>("Words");
			});


			builder.Services.AddSingleton<MainPageViewModel>();
			builder.Services.AddSingleton<MainPage>();

			builder.Services.AddSingleton<WordAddPageViewModel>();
			builder.Services.AddSingleton<WordAddPage>();

			builder.Services.AddSingleton<WordListViewModel>();
			builder.Services.AddSingleton<WordListPage>();

			builder.Services.AddSingleton<IWordService, WordServiceMongo>();
			builder.Services.AddSingleton<IDeletedWordLogger, DeletedWordLoggerJson>();

			return builder.Build();
		}
	}
}
