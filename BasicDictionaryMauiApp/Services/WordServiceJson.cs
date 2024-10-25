using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Models.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BasicDictionaryMauiApp.Services
{
    public class WordServiceJson : JsonBaseClass, IWordService
	{
		public WordServiceJson()
		{
			JsonFileName = Path.Combine(FileSystem.AppDataDirectory, "words.json");
			JsonOptions = new JsonSerializerOptions
			{
				WriteIndented = false,
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
			};
		}

		public WordServiceJson(string jsonFileName)
		{
			JsonFileName = jsonFileName;
			JsonOptions = new JsonSerializerOptions
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

			await SaveToJsonFile(wordList);

			return word;
		}

		public async Task<int> CountWordsAsync()
		{
			var words = await GetWordsAsync();
			return words.Count();
		}

		public async Task<PagedResult<WordPagedItemModel>> GetPagedWordsAsync(int currentPage, int pageSize)
		{
			if (currentPage < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(currentPage), "Current page must be greater than 0.");
			}

			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
			}

			var words = await GetWordsFromJsonAsync<WordPagedItemModel>();

			var totalItems = words.Count();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			if (currentPage > totalPages)
			{
				return new PagedResult<WordPagedItemModel>
				{
					Items = [],
					TotalItems = totalItems
				};
			}

			var pagedWords = words
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize);

			return new PagedResult<WordPagedItemModel>
			{
				Items = pagedWords,
				TotalItems = totalItems
			};
		}

		public async Task<IEnumerable<WordModel>> GetWordsAsync()
		{
			return await GetWordsFromJsonAsync<WordModel>();
		}

		public async Task<WordModel> RemoveWordAsync(Guid id)
		{
			var words = await GetWordsAsync();
			var word = words.FirstOrDefault(q => q.Id == id);
			if (word != null)
			{
				var wordList = words.Where(q => q.Id != id).ToList();
				await SaveToJsonFile(wordList);
			}

			return word;
		}

		public async Task<PagedResult<WordPagedItemModel>> SearchWords(string name)
		{
			var words = await GetWordsFromJsonAsync<WordPagedItemModel>();
			var foundWords = words
				.Where(q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
				.ToList();

			return new PagedResult<WordPagedItemModel>
			{
				Items = foundWords,
				TotalItems = words.Count()
			};
		}

		private void G()
		{

		}
	}
}

