using BasicDictionaryMauiApp.Models.Entities;

namespace BasicDictionaryMauiApp.Events;

public class WordChangedEventArgs(WordModel word) : EventArgs
{
	public WordModel Word { get; } = word;
}
