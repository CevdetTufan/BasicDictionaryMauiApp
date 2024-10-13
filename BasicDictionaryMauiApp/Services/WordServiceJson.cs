using BasicDictionaryMauiApp.Models;
using System.Text.Json;

namespace BasicDictionaryMauiApp.Services
{
	public class WordServiceJson : IWordService
	{
		private readonly string _jsonFileName;

		public WordServiceJson(string jsonFileName)
		{
			_jsonFileName = jsonFileName;
		}

		public async Task<WordModel> AddWordAsync(WordModel word)
		{
			var words = await GetWordsAsync();
			var wordList = words.ToList();    
			wordList.Add(word);               

			using (var stream = File.Create(_jsonFileName))
			{
				await JsonSerializer.SerializeAsync(stream, wordList);
			}

			return word;
		}

		public async Task<IEnumerable<WordModel>> GetWordsAsync()
		{
			if (!File.Exists(_jsonFileName))
			{
				return []; 
			}

			using (var stream = File.OpenRead(_jsonFileName))
			{
				var words = await JsonSerializer.DeserializeAsync<List<WordModel>>(stream);
				return words ?? []; 
			}
		}
	}
}

