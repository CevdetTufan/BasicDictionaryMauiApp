using BasicDictionaryMauiApp.Models;
using BasicDictionaryMauiApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BasicDictionaryMauiApp.ViewModels
{
	public class WordListViewModel : INotifyPropertyChanged
	{
		private readonly IWordService _wordService;
		private readonly IDeletedWordLogger _deletedWordLogger;

		private const int PageSize = 10;
		private int _currentPage = 1;
		private bool _isLoading;

		public WordListViewModel(IWordService wordService, IDeletedWordLogger deletedWordLogger)
		{
			_wordService = wordService;
			_deletedWordLogger = deletedWordLogger;
			LoadMoreCommand = new Command(async () => await LoadMoreWordsAsync());
			RemoveWordCommand = new Command<WordPagedItemModel>(async (word) => await RemoveWordAsync(word));
			LoadMoreCommand.Execute(this);
		}

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPage != value)
				{
					_currentPage = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
				}
			}
		}

		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					OnPropertyChanged(nameof(IsLoading));
				}
			}
		}

		public ObservableCollection<WordPagedItemModel> Words { get; set; } = [];


		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ICommand LoadMoreCommand { get; }
		public async Task LoadMoreWordsAsync()
		{
			IsLoading = true;
			var pagedList = await _wordService.GetPagedWordsAsync(CurrentPage, PageSize);
			foreach (var word in pagedList.Items)
			{
				Words.Add(word);
			}

			CurrentPage++;
			IsLoading = false;
		}

		public ICommand RemoveWordCommand { get; }

		public async Task RemoveWordAsync(WordPagedItemModel word)
		{
			var deletedWord = await _wordService.RemoveWordAsync(word.Id);
			if (deletedWord != null)
			{
				await _deletedWordLogger.LogDeletedWordAsync(deletedWord);
				Words.Remove(word);
			}
		}
	}
}
