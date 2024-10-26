using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Models.Entities;
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
		private MessageModel _message;

		public string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
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
					OnPropertyChanged(nameof(Meaning));
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

		public MessageModel Message
		{
			get => _message;
			set
			{
				if (_message != value)
				{
					_message = value;
					OnPropertyChanged(nameof(Message));
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
					Id = Guid.NewGuid(),
					Name = Name,
					Meaning = Meaning,
					Definition = Definition,
					CreatedTime = DateTime.UtcNow
				};

				await _wordService.AddWordAsync(newWord);

				Name = string.Empty;
				Meaning = string.Empty;
				Definition = string.Empty;

				Message = new MessageModel
				{
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				//todo : log error

				Message = new MessageModel
				{
					IsSuccess = false,
					Message = "An error occurred."
				};
			}
		}
	}
}
