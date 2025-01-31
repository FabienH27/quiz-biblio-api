using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models.Quiz;

[Collection("Quizzes")]
public class QuizEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("themes")]
    public List<string> Themes { get; set; } = [];

    [BsonElement("imageId")]
    public string? ImageId { get; set; }

    [BsonElement("questions")]
    public List<Question> Questions { get; set; } = [];

    [BsonElement("creator")]
    public required QuizCreator Creator { get; set; }

}