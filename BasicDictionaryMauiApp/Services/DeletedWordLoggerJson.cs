using BasicDictionaryMauiApp.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BasicDictionaryMauiApp.Services
{
	public class DeletedWordLoggerJson : JsonBaseClass, IDeletedWordLogger
	{
		public DeletedWordLoggerJson()
		{
			JsonFileName = Path.Combine(FileSystem.AppDataDirectory, "deletedwords.json");
			JsonOptions = new JsonSerializerOptions
			{
				WriteIndented = false,
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
			};
		}

		public async Task LogDeletedWordAsync(WordModel deletedWord)
		{
			var deletedWords = await GetWordsFromJsonAsync<DeletedWordModel>();

			var words = deletedWords.ToList();
			words.Add(new DeletedWordModel
			{
				Definition = deletedWord.Definition,
				Id = deletedWord.Id,
				Meaning = deletedWord.Meaning,
				Name = deletedWord.Name,
				DeletedTime = DateTime.UtcNow
			});


			await SaveToJsonFile(words);
		}
	}
}
