﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuizBiblio.Models.Rbac;

namespace QuizBiblio.Models;

public class UserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("Password")]
    public required string Password {  get; set; }

    [BsonElement("Username")]
    public required string Username { get; set; }

    [BsonElement("Role")]
    public string UserRole { get; set; } = Role.Member.Name;
}