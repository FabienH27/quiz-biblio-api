using MongoDB.Bson;

namespace QuizBiblio.Models.Quiz
{
    public class QuizCreator
    {
        public ObjectId CreatorId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
