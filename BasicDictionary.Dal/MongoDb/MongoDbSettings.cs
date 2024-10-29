namespace BasicDictionary.Dal.MongoDb;

public class MongoDbSettings
{
	public string ConnectionString { get; init; }
	public string DatabaseName { get; init; }

	public MongoDbSettings(string connectionString, string databaseName)
	{
		ConnectionString = connectionString;
		DatabaseName = databaseName;
	}
}