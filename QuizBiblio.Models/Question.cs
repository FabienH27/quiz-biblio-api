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
    public required string ImageId { get; set; }

    [BsonElement("answer")]
    public int CorrectAnswerIndex { get; set; }

    [BsonElement("options")]
    public List<Option> Options { get; set; } = [];

}
