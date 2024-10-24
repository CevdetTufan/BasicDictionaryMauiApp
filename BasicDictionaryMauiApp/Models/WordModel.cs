namespace BasicDictionaryMauiApp.Models;

public class WordModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Meaning { get; set; }
    public string Definition { get; set; }
    public DateTime? CreatedTime { get; set; }
}

public class DeletedWordModel : WordModel
{
    public DateTime? DeletedTime { get; set; }
}
