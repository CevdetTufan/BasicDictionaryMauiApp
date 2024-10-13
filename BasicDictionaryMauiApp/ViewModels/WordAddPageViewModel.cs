using BasicDictionaryMauiApp.Models;
using BasicDictionaryMauiApp.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace BasicDictionaryMauiApp.ViewModels
{
	public class WordAddPageViewModel : INotifyPropertyChanged
	{
		private readonly IWordService _wordService;

		private string _name = string.Empty;
		private string _meaning = string.Empty;
		private string _definition = string.Empty;

		private string _errorMessage = string.Empty;
		private bool _isSuccess = false;

		public string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
				}
			}
		}

		public string Meaning
		{
			get => _meaning;
			set
			{
				if (_meaning != value)
				{
					_meaning = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Meaning)));
				}
			}
		}

		public string Definition
		{
			get => _definition;
			set
			{
				if (_definition != value)
				{
					_definition = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Definition)));
				}
			}
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				if (_errorMessage != value)
				{
					_errorMessage = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
				}
			}
		}

		public bool IsSuccess
		{
			get => _isSuccess;
			set
			{
				if (_isSuccess != value)
				{
					_isSuccess = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSuccess)));
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ICommand AddWordCommand { get; }

        public WordAddPageViewModel(IWordService wordService)
        {
			AddWordCommand = new Command(AddWordItem);
			_wordService = wordService;
		}

		private async void AddWordItem()
		{
			try
			{
				var newWord = new WordModel
				{
					Name = Name,
					Meaning = Meaning,
					Definition = Definition
				};

				await _wordService.AddWordAsync(newWord);

				Name = string.Empty;
				Meaning = string.Empty;
				Definition = string.Empty;
				IsSuccess = true;
			}
			catch (Exception)
			{
				//todo : log error
				ErrorMessage = "Hata Meydana Geldi.";
			}
		}
    }
}
