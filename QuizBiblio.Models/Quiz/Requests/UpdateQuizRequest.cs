namespace QuizBiblio.Models.Quiz.Requests;

public class UpdateQuizRequest
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public List<string> Themes { get; set; } = [];

    public string? ImageId { get; set; }

    public List<Question> Questions { get; set; } = [];
}
