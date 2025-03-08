namespace BasicDictionaryMauiApp.Models.Dtos
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItems { get; set; }
    }
}
