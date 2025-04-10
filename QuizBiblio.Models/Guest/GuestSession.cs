using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Models.Guest;

public class GuestSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? UserName { get; set; }

    public QuizAnswerDto? Answers { get; set; }
 
    public GuestSession(string userName)
    {
        UserName = userName;
    }
}
