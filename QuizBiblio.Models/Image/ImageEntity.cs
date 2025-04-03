using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace QuizBiblio.Models.Image;

public class ImageEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required string OriginalUrl { get; set; }

    public string ResizedUrl { get; set; } = string.Empty;

    public bool? IsPermanent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
