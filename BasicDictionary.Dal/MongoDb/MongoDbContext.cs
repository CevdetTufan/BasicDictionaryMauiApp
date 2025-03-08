using MongoDB.Bson;
using MongoDB.Driver;

namespace BasicDictionary.Dal.MongoDb;

public class MongoDbContext
{
	private readonly IMongoDatabase _database;

	public MongoDbContext(MongoDbSettings setting)
	{

		var settings = MongoClientSettings.FromConnectionString(setting.ConnectionString);
		settings.ServerApi = new ServerApi(ServerApiVersion.V1);

		var client = new MongoClient(settings);

		// Send a ping to confirm a successful connection
		try
		{
			var result = client.GetDatabase(setting.DatabaseName).RunCommand<BsonDocument>(new BsonDocument("ping", 1));

			_database = client.GetDatabase(setting.DatabaseName);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public IMongoCollection<T> GetCollection<T>(string collectionName)
	{
		return _database.GetCollection<T>(collectionName);
	}
}
