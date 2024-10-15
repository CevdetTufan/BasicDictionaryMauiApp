namespace BasicDictionaryMauiApp.Models
{
	public class PagedResult<T>
	{
		public IEnumerable<T> Items { get; set; }
		public int TotalPages { get; set; }
	}
}
