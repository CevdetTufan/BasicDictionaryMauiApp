using BasicDictionaryMauiApp.Models;

namespace BasicDictionaryMauiApp.Services
{
	public interface IWordService : IPagitableList<WordPagedItemModel>
	{
		Task<IEnumerable<WordModel>> GetWordsAsync();
		Task<WordModel> AddWordAsync(WordModel word);
		Task<WordModel> RemoveWordAsync(Guid id);	
		Task<int> CountWordsAsync();
	}
}
