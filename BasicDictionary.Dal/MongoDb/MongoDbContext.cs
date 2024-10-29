using MongoDB.Driver;

namespace BasicDictionary.Dal.MongoDb;

public class MongoDbContext
{
	private readonly IMongoDatabase _database;

	public MongoDbContext(MongoDbSettings settings)
	{
		var client = new MongoClient(settings.ConnectionString);
		_database = client.GetDatabase(settings.DatabaseName);
	}

	public IMongoCollection<T> GetCollection<T>(string collectionName)
	{
		return _database.GetCollection<T>(collectionName);
	}
}
