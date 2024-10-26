using BasicDictionaryMauiApp.Models.Dtos;

namespace BasicDictionaryMauiApp.Services
{
    public interface IPagitableList<T> 
	{
		Task<PagedResult<T>> GetPagedWordsAsync(int currentPage, int pageSize);
		Task<PagedResult<WordPagedItemModel>> SearchWords(string name, int currentPage, int pageSize);
	}
}
