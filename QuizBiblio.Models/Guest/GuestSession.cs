using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Models.Guest;

public class GuestSession(string userName)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? UserName { get; set; } = userName;

    public int Score { get; set; }
}
