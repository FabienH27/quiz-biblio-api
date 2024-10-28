using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models;

public class Question
{
    [BsonElement("text")]
    public required string Text { get; set; }

    [BsonElement("explanation")]
    public string Explanation { get; set; } = string.Empty;

    [BsonElement("imageId")]
    public string? ImageId { get; set; }

    [BsonElement("correctAnswerIndex")]
    public List<int> correctAnswerIndex { get; set; } = [];

    [BsonElement("options")]
    public List<Option> Options { get; set; } = [];

}
