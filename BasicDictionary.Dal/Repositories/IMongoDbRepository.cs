using System.Linq.Expressions;

namespace BasicDictionary.Dal.Repositories;

public interface IMongoDbRepository<T>: IRepository<T> where T : class
{
	Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter);
}
