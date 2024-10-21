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

		private const int PageSize = 10;
		private int _currentPage = 1;
		private bool _isLoading;

		public event PropertyChangedEventHandler? PropertyChanged;

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

		public ICommand LoadMoreCommand { get; }

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public WordListViewModel(IWordService wordService)
		{
			_wordService = wordService;
			LoadMoreCommand = new Command(async () => await LoadMoreWordsAsync());
			LoadMoreCommand.Execute(this);
		}

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
	}
}
