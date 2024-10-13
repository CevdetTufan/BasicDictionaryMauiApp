using BasicDictionaryMauiApp.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BasicDictionaryMauiApp.Services
{
	public class WordServiceJson : IWordService
	{
		private readonly string _jsonFileName;

		private readonly JsonSerializerOptions _jsonOptions;

		public WordServiceJson(string jsonFileName)
		{
			_jsonFileName = jsonFileName;

			_jsonOptions = new JsonSerializerOptions
			{
				WriteIndented = false,
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
			};
		}

		public async Task<WordModel> AddWordAsync(WordModel word)
		{
			var words = await GetWordsAsync();
			var wordList = words.ToList();

			if (wordList.Exists(w => w.Name.Equals(word.Name, StringComparison.CurrentCultureIgnoreCase)))
			{
				throw new InvalidOperationException("Word already exists.");
			}

			wordList.Add(word);

			using (var stream = File.Create(_jsonFileName))
			{
				await JsonSerializer.SerializeAsync(stream, wordList, _jsonOptions);
			}

			return word;
		}

		public async Task<IEnumerable<WordModel>> GetWordsAsync()
		{
			if (!File.Exists(_jsonFileName))
			{
				return [];
			}

			using var stream = File.OpenRead(_jsonFileName);
			var words = await JsonSerializer.DeserializeAsync<List<WordModel>>(stream, _jsonOptions);
			return words ?? [];
		}
	}
}

