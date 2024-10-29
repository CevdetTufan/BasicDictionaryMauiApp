using System.Linq.Expressions;

namespace BasicDictionary.Dal.Repositories;

public interface IRepository<T> where T : class
{
	Task<T> GetByIdAsync(Guid id);
	Task<IEnumerable<T>> GetAllAsync();
	Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);
	Task AddAsync(T entity);
	Task UpdateAsync(Guid id, T entity);
	Task DeleteAsync(Guid id);
	Task<int> CountAsync();
	Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> filter, int skip, int take);
}