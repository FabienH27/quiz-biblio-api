using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Models;

public class User
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("Password")]
    public required string Password {  get; set; }

    [BsonElement("Username")]
    public required string Username { get; set; }
}
