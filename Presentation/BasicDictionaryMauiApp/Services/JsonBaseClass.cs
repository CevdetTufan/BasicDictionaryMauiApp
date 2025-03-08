using System.Text.Json;

namespace BasicDictionaryMauiApp.Services;

public abstract class JsonBaseClass
{
	protected JsonSerializerOptions JsonOptions { get; set; }
	protected string JsonFileName { get; set; }


	protected async Task SaveToJsonFile<T>(List<T> wordList)
	{
		using var stream = File.Create(JsonFileName);
		await JsonSerializer.SerializeAsync(stream, wordList, JsonOptions);
	}

	protected async Task<IEnumerable<T>> GetWordsFromJsonAsync<T>()
	{
		if (!File.Exists(JsonFileName))
		{
			return [];
		}

		using var stream = File.OpenRead(JsonFileName);
		var words = await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions);
		return words ?? [];
	}
}
