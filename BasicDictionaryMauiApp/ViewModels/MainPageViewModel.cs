using BasicDictionaryMauiApp.Models.Dtos;
using BasicDictionaryMauiApp.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace BasicDictionaryMauiApp.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
	private readonly IWordService _wordService;
	private MainPageQueueItemModel _dequeueItem;
	private readonly Queue<MainPageQueueItemModel> queue = new();

	private int _nextButtonClickCount;
	private int _queueCount;

	public MainPageViewModel(IWordService wordService)
	{
		_wordService = wordService;
		DequeeCommand = new Command(async () => await DequeeAsync());
		DequeeCommand.Execute(true);
	}

	public MainPageQueueItemModel DequeueItem
	{
		get => _dequeueItem;
		set
		{
			if (_dequeueItem != value)
			{
				_dequeueItem = value;
				OnPropertyChanged(nameof(DequeueItem));
			}
		}
	}

	public int NextButtonClickCount
	{
		get { return _nextButtonClickCount; }
		set
		{
			if (value != _nextButtonClickCount)
			{
				_nextButtonClickCount = value;
				OnPropertyChanged(nameof(NextButtonClickCount));
			}
		}
	}

	public int QueueCount
	{
		get { return _queueCount; }
		set
		{
			if (value != _queueCount)
			{
				_queueCount = value;
				OnPropertyChanged(nameof(QueueCount));
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public readonly ICommand DequeeCommand;

	private async Task DequeeAsync()
	{
		try
		{
			if (queue.Count == 0)
			{
				await FillQueue();
				NextButtonClickCount = 0;
				QueueCount = queue.Count;
			}

			NextButtonClickCount++;
			DequeueItem = queue.Dequeue();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred during search: {ex.Message}");
		}
	}

	private async Task FillQueue()
	{
		var words = await _wordService.GetWordsAsync();
		foreach (var word in words)
		{
			queue.Enqueue(new MainPageQueueItemModel
			{
				Definition = word.Definition,
				Meaning = word.Meaning,
				Name = word.Name
			});
		}
	}
}
