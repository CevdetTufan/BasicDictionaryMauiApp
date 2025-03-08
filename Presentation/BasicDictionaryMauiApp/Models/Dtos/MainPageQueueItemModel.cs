namespace BasicDictionaryMauiApp.Models.Dtos;

public class MainPageQueueItemModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Meaning { get; set; }
	public string Definition { get; set; }

	public List<SampleModelItemModel> Samples { get; set; } = [];
}

public class SampleModelItemModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}
