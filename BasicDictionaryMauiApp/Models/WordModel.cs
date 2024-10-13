namespace BasicDictionaryMauiApp.Models;

public class WordModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Meaning { get; set; }
    public string? Definition { get; set; }
}
