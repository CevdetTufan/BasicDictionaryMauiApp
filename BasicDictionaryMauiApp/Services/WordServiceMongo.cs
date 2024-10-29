using BasicDictionary.Dal.Repositories;
using BasicDictionaryMauiApp.Events;
using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Models.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace BasicDictionaryMauiApp.Services;

public class WordServiceMongo : IWordService
{
	private readonly IMongoDbRepository<WordModel> _repository;

	public event EventHandler<WordChangedEventArgs> WordAdded;
	public event EventHandler<WordChangedEventArgs> WordRemoved;

	public WordServiceMongo(IMongoDbRepository<WordModel> repository)
	{
		_repository = repository;
	}

	public async Task<WordModel> AddWordAsync(WordModel word)
	{
		await _repository.AddAsync(word);
		WordAdded?.Invoke(this, new WordChangedEventArgs(word));
		return word;
	}

	public async Task<int> CountWordsAsync()
	{
		var words = await _repository.GetAllAsync();
		return words.Count();
	}

	public async Task<PagedResult<WordPagedItemModel>> GetPagedWordsAsync(int currentPage, int pageSize)
	{
		return await FetchPagedWords(string.Empty, currentPage, pageSize);
	}

	public async Task<IEnumerable<WordModel>> GetWordsAsync()
	{
		var words = await _repository.GetAllAsync();
		return words;
	}

	public async Task<WordModel> RemoveWordAsync(Guid id)
	{
		var word = await _repository.GetByIdAsync(id);
		if (word != null)
		{
			await _repository.DeleteAsync(id);

			WordRemoved?.Invoke(this, new WordChangedEventArgs(word));
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

	private async Task<PagedResult<WordPagedItemModel>> FetchPagedWords(string name, int currentPage, int pageSize)
	{
		Expression<Func<WordModel, bool>> filter = w => w.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

		var totalWords = await _repository.CountDocumentsAsync(filter);

		var words = await _repository.GetPagedAsync(filter, (currentPage - 1) * pageSize, pageSize);

		var wordItems = words.Select(word => new WordPagedItemModel
		{
			Id = word.Id,
			Name = word.Name,
			Meaning = word.Meaning
		}).ToList();

		var pagedResult = new PagedResult<WordPagedItemModel>
		{
			TotalItems = (int)totalWords,
			Items = wordItems
		};

		return pagedResult;
	}
}
