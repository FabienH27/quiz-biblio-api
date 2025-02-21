using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizBiblio.Models;

public class UserEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("Password")]
    public required string Password {  get; set; }

    [BsonElement("Username")]
    public required string Username { get; set; }

    [BsonElement("Role")]
    public string Role { get; set; } = "USER";
}
