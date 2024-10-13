using BasicDictionaryMauiApp.Models;

namespace BasicDictionaryMauiApp.Services
{
	public interface IWordService
	{
		Task<IEnumerable<WordModel>> GetWordsAsync();
		Task<WordModel> AddWordAsync(WordModel word);
	}
}
