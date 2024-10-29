using BasicDictionary.Dal.MongoDb;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BasicDictionary.Dal.Repositories;

public class MongoDbRepository<T> : IMongoDbRepository<T> where T : class
{
	private readonly IMongoCollection<T> _collection;

	public MongoDbRepository(MongoDbContext context)
	{
		var displayName = GetDisplayName() ?? typeof(T).Name;
		_collection = context.GetCollection<T>(displayName);
	}

	public async Task<T> GetByIdAsync(Guid id)
	{
		return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _collection.Find(_ => true).ToListAsync();
	}

	public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
	{
		return await _collection.Find(filter).ToListAsync();
	}

	public async Task AddAsync(T entity)
	{
		await _collection.InsertOneAsync(entity);
	}

	public async Task UpdateAsync(Guid id, T entity)
	{
		await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);
	}

	public async Task DeleteAsync(Guid id)
	{
		await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
	}

	public async Task<int> CountAsync()
	{
		return (int)await _collection.CountDocumentsAsync(_ => true);
	}

	public async Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter)
	{
		return await _collection.CountDocumentsAsync(filter);
	}

	public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> filter, int skip, int take)
	{
		return await _collection.AsQueryable()
								 .Where(filter)
								 .Skip(skip)
								 .Take(take)
								 .ToListAsync();
	}

	private static string GetDisplayName()
	{
		var attr = typeof(T).GetCustomAttributes(typeof(DisplayNameAttribute), false)
			.FirstOrDefault() as DisplayNameAttribute;

		return attr?.DisplayName;
	}
}
