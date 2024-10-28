using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Models;

public class Option
{
    [BsonElement("text")]
    public string Text { get; set; } = string.Empty;

    [BsonElement("imageId")]
    public required string ImageId { get; set; }
}
