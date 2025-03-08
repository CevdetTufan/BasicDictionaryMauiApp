using BasicDictionaryMauiApp.Events;
using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Models.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BasicDictionaryMauiApp.Services
{
	public class WordServiceJson : JsonBaseClass, IWordService
	{
		private List<WordModel> _cachedWords;
		private DateTime _lastCacheUpdateTime;
		private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

		public event EventHandler<WordChangedEventArgs> WordAdded;
		public event EventHandler<WordChangedEventArgs> WordRemoved;

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
			var words = await LoadWordsFromJsonAsync();

			if (words.Exists(w => w.Name.Equals(word.Name, StringComparison.CurrentCultureIgnoreCase)))
			{
				throw new InvalidOperationException("Word already exists.");
			}

			words.Add(word);

			await SaveToJsonFile(words);

			_cachedWords = words;
			_lastCacheUpdateTime = DateTime.UtcNow;

			OnWordAdded(word);

			return word;
		}

		public async Task<int> CountWordsAsync()
		{
			var words = await LoadWordsFromJsonAsync();
			return words.Count;
		}

		public async Task<PagedResult<WordPagedItemModel>> GetPagedWordsAsync(int currentPage, int pageSize)
		{
			return await FetchPagedWords(string.Empty, currentPage, pageSize);
		}

		public async Task<IEnumerable<WordModel>> GetWordsAsync()
		{
			return await LoadWordsFromJsonAsync();
		}

		public async Task<WordModel> RemoveWordAsync(Guid id)
		{
			var words = await LoadWordsFromJsonAsync();
			var word = words.Find(q => q.Id == id);
			if (word != null)
			{
				var updatedWords = words.Where(q => q.Id != id).ToList();
				await SaveToJsonFile(updatedWords);

				_cachedWords = updatedWords;
				_lastCacheUpdateTime = DateTime.UtcNow;

				OnWordRemoved(word);
			}

			return word;
		}

		public async Task<PagedResult<WordPagedItemModel>> SearchWords(string name, int currentPage, int pageSize)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return new PagedResult<WordPagedItemModel>
				{
					Items = [],
					TotalItems = 0
				};
			}

			return await FetchPagedWords(name, currentPage, pageSize);
		}

		#region Event Handlers
		protected virtual void OnWordAdded(WordModel word)
		{
			WordAdded?.Invoke(this, new WordChangedEventArgs(word));
		}

		protected virtual void OnWordRemoved(WordModel word)
		{
			WordRemoved?.Invoke(this, new WordChangedEventArgs(word));
		}

		#endregion

		#region Helper Methods
		private async Task<List<WordModel>> LoadWordsFromJsonAsync()
		{
			if (_cachedWords == null || DateTime.UtcNow - _lastCacheUpdateTime > _cacheDuration)
			{
				_cachedWords = (await GetWordsFromJsonAsync<WordModel>()).ToList();
				_lastCacheUpdateTime = DateTime.UtcNow;
			}
			return _cachedWords;
		}

		private async Task<PagedResult<WordPagedItemModel>> FetchPagedWords(string name, int currentPage, int pageSize)
		{
			if (currentPage < 1) throw new ArgumentOutOfRangeException(nameof(currentPage), "Current page must be greater than 0.");
			if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

			var words = await LoadWordsFromJsonAsync();
			var pagedWords = words
				.Where(q => q.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
				.Select(word => new WordPagedItemModel
				{
					Id = word.Id,
					Name = word.Name,
					Meaning = word.Meaning
				})
				.Skip((currentPage - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return new PagedResult<WordPagedItemModel>
			{
				Items = pagedWords,
				TotalItems = words.Count
			};
		}

		#endregion
	}
}

