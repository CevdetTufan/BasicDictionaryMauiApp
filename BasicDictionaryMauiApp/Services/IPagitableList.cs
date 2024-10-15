using BasicDictionaryMauiApp.Models;

namespace BasicDictionaryMauiApp.Services
{
	public interface IPagitableList<T> 
	{
		Task<PagedResult<T>> GetPagedWordsAsync(int currentPage, int pageSize);
	}
}
