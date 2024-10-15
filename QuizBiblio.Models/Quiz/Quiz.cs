using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models.Quiz;

[Collection("Quizzes")]
public class Quiz
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("themeIds")]
    public List<ObjectId> ThemeIds { get; set; } = [];
}
