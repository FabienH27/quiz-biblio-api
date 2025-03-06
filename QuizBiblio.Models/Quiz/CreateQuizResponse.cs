namespace QuizBiblio.Models.Quiz;

public class CreateQuizResponse
{
    public required string Title { get; set; }

    public List<string> Themes { get; set; } = [];

    public string? Image { get; set; }

    public List<Question> Questions { get; set; } = [];
}
