using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizBiblio.Models.Quiz
{
    public class QuizCreator
    {
        [BsonElement("id")]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
    }
}
