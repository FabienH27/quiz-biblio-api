namespace QuizBiblio.Models.Quiz;

public class QuizInfo
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public List<string> Themes { get; set; } = [];

    public string? Image { get; set; }

    public int QuestionCount { get; set; }

    public required string CreatorName { get; set; }
}