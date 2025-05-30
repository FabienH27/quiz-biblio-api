﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace QuizBiblio.Models;

public class Question
{
    [BsonElement("text")]
    public required string Text { get; set; }

    [BsonElement("details")]
    public string Details { get; set; } = string.Empty;

    [BsonElement("imageId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ImageId { get; set; }

    [BsonElement("correctProposalIds")]
    public List<int> CorrectProposalIds { get; set; } = [];

    [BsonElement("proposals")]
    public List<Proposal> Proposals { get; set; } = [];

}
