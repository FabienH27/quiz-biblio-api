using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace QuizBiblio.Models.UserQuiz;

[Collection("UserQuizScore")]
public class UserQuizScoreEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; }

    [BsonElement("UserName")]
    public required string UserName { get; set; }

    [BsonElement("Score")]
    public int Score { get; set; }
}
