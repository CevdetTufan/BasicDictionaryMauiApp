using BasicDictionaryMauiApp.Models.Entities;

namespace BasicDictionaryMauiApp.Services
{
    public interface IDeletedWordLogger
    {
		Task LogDeletedWordAsync(WordModel deletedWord);
	}
}
