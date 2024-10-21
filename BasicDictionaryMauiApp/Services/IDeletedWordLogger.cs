using BasicDictionaryMauiApp.Models;

namespace BasicDictionaryMauiApp.Services
{
	public interface IDeletedWordLogger
    {
		Task LogDeletedWordAsync(WordModel deletedWord);
	}
}
