using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models;

[Collection("Themes")]
public class Theme
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("title")]
    public required string Name { get; set; }
}
