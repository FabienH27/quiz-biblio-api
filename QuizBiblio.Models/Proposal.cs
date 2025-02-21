using MongoDB.Bson.Serialization.Attributes;

namespace QuizBiblio.Models;

public class Proposal
{
    [BsonElement("text")]
    public string Text { get; set; } = string.Empty;

    [BsonElement("imageId")]
    public string? ImageId { get; set; }
}
