using BasicDictionaryMauiApp.Events;
using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Models.Entities;

namespace BasicDictionaryMauiApp.Services
{
    public interface IWordService : IPagitableList<WordPagedItemModel>
	{
		Task<IEnumerable<WordModel>> GetWordsAsync();
		Task<WordModel> AddWordAsync(WordModel word);
		Task<WordModel> RemoveWordAsync(Guid id);	
		Task<int> CountWordsAsync();

		event EventHandler<WordChangedEventArgs> WordAdded;
		event EventHandler<WordChangedEventArgs> WordRemoved;
	}
}
