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
		private int _totalItems;
		private int _showingItems;

		public WordListViewModel(IWordService wordService, IDeletedWordLogger deletedWordLogger)
		{
			_wordService = wordService;
			_deletedWordLogger = deletedWordLogger;
			LoadMoreCommand = new Command(async () => await LoadMoreWordsAsync());
			RemoveWordCommand = new Command<WordPagedItemModel>(async (word) => await RemoveWordAsync(word));
			SearchWordsCommand = new Command<string>(async (name) => await SearchWords(name));
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

		public int TotalItems
		{
			get => _totalItems;
			set
			{
				if (_totalItems != value)
				{
					_totalItems = value;
					OnPropertyChanged(nameof(TotalItems));	
				}
			}
		}

		public int ShowingItems
		{
			get=> _showingItems;
			set
			{
				if(_showingItems != value)
				{
					_showingItems = value;
					OnPropertyChanged(nameof(ShowingItems));
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

			if (CurrentPage == 1)
				Words.Clear();

			var pagedList = await _wordService.GetPagedWordsAsync(CurrentPage, PageSize);
			foreach (var word in pagedList.Items)
			{
				Words.Add(word);
			}

			CurrentPage++;

			TotalItems = pagedList.TotalItems;
			ShowingItems = Words.Count;
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
				TotalItems--;
				ShowingItems= Words.Count;
			}
		}

		public ICommand SearchWordsCommand { get; }

		public async Task SearchWords(string name)
		{
			ClearProperties();
			var foundWords = await _wordService.SearchWords(name);
			foreach (var word in foundWords.Items)
			{
				Words.Add(word);
			}

			TotalItems= foundWords.TotalItems;
			ShowingItems = Words.Count;
		}

		public void ClearProperties()
		{
			Words.Clear();
			CurrentPage = 1;
			TotalItems = 0;
			ShowingItems= 0;
		}
	}
}
