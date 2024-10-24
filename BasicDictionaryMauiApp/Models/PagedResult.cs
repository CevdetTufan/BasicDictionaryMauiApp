namespace BasicDictionaryMauiApp.Models
{
	public class PagedResult<T>
	{
		public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
