using BasicDictionaryMauiApp.Models.Dtos;
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
			SetTotalItemsInitialCountCommand = new Command(async () => await SetTotalItemsInitialCountAsync());
			ClearPropertiesCommand = new Command(ClearProperties);
		}

		#region Properties

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPage != value)
				{
					_currentPage = value;
					OnPropertyChanged(nameof(CurrentPage));
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
			get => _showingItems;
			set
			{
				if (_showingItems != value)
				{
					_showingItems = value;
					OnPropertyChanged(nameof(ShowingItems));
				}
			}
		}

		public ObservableCollection<WordPagedItemModel> Words { get; set; } = []; 
		#endregion

		#region Commands
		public ICommand LoadMoreCommand { get; }
		public async Task LoadMoreWordsAsync()
		{
			IsLoading = true;
			try
			{
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
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred during search: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		public ICommand RemoveWordCommand { get; }

		public async Task RemoveWordAsync(WordPagedItemModel word)
		{
			try
			{
				var deletedWord = await _wordService.RemoveWordAsync(word.Id);
				if (deletedWord != null)
				{
					await _deletedWordLogger.LogDeletedWordAsync(deletedWord);
					Words.Remove(word);
					TotalItems--;
					ShowingItems = Words.Count;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred during search: {ex.Message}");
			}
		}

		public ICommand SearchWordsCommand { get; }

		public async Task SearchWords(string name, int currentPage = 1, int pageSize = 10)
		{
			try
			{
				ClearProperties();

				var foundWords = await _wordService.SearchWords(name, CurrentPage, PageSize);

				Words.Clear();
				foreach (var word in foundWords.Items)
				{
					Words.Add(word);
				}

				TotalItems = foundWords.TotalItems;
				ShowingItems = Words.Count;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred during search: {ex.Message}");
			}
		}

		public ICommand SetTotalItemsInitialCountCommand {  get; }

		private async Task SetTotalItemsInitialCountAsync()
		{
			TotalItems = await _wordService.CountWordsAsync();
		}

		public ICommand ClearPropertiesCommand {  get; }
		private void ClearProperties()
		{
			Words.Clear();
			CurrentPage = 1;
			TotalItems = 0;
			ShowingItems = 0;
		}

		#endregion

		#region Event Handlers

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
