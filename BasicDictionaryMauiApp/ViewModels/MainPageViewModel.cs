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
		_wordService.WordAdded += OnWordServiceWordAdded;
		_wordService.WordRemoved += OnWordServiceWordRemoved;
	}

	#region Properties

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
			if (value != _nextButtonClickCount && value >= 0 && value <= QueueCount)
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
			if (value != _queueCount && value >= 0)
			{
				_queueCount = value;
				OnPropertyChanged(nameof(QueueCount));
			}
		}
	}

	#endregion

	#region Commands

	public readonly ICommand DequeeCommand;
	private async Task DequeeAsync()
	{
		try
		{
			if (queue.Count == 0)
			{
				await EnqueueItems();
				NextButtonClickCount = 0;
				QueueCount = queue.Count;
			}

			NextButtonClickCount++;
			Dequeue();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred during search: {ex.Message}");
		}
	}
	#endregion

	#region Event Handlers

	public event PropertyChangedEventHandler PropertyChanged;
	public void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void OnWordServiceWordAdded(object sender, Events.WordChangedEventArgs e)
	{
		queue.Enqueue(new MainPageQueueItemModel
		{
			Definition = e.Word.Definition,
			Meaning = e.Word.Meaning,
			Name = e.Word.Name
		});

		QueueCount++;
	}
	private void OnWordServiceWordRemoved(object sender, Events.WordChangedEventArgs e)
	{
		var itemsToRemove = queue.Where(q => q.Name == e.Word.Name).ToList();

		foreach (var item in itemsToRemove)
		{
			if (queue.Peek().Name == item.Name)
			{
				queue.Dequeue();
			}
		}

		QueueCount--;

		if (NextButtonClickCount > QueueCount)
		{
			NextButtonClickCount = QueueCount;
		}
	}

	#endregion

	#region Helper Methods
	private async Task EnqueueItems()
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

	private void Dequeue()
	{
		if (queue.Count > 0)
		{
			DequeueItem = queue.Dequeue();
		}
		else
		{
			DequeueItem = null;
		}
	}
	#endregion
}
