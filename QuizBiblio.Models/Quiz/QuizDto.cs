using MongoDB.Bson;

namespace QuizBiblio.Models.Quiz;

public class QuizDto
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required string Title { get; set; }

    public List<string> Themes { get; set; } = [];

    public string? ImageId { get; set; }

    public IEnumerable<Question> Questions { get; set; } = [];

    public required QuizCreator Creator { get; set; }
}
