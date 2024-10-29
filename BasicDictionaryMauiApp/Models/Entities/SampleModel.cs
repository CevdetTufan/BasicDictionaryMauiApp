using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace BasicDictionaryMauiApp.Models.Entities;

[DisplayName("Samples")]
public class SampleModel
{
	[BsonGuidRepresentation(GuidRepresentation.Standard)]
	[BsonId]
	public Guid Id { get; set; }
	public string Name { get; set; }
	public DateTime CreatedTime { get; set; }
}
