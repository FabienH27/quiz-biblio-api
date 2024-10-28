using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models;

[Collection("Quizzes")]
public class Quiz
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("themes")]
    public List<string> Themes { get; set; } = [];

    [BsonElement("imageId")]
    public string? ImageId { get; set; }

    [BsonElement("questions")]
    public List<Question> Questions { get; set; } = [];
}
