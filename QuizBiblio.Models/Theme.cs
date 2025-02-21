using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace QuizBiblio.Models;

public class Theme
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }
}
