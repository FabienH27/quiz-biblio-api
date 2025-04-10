namespace QuizBiblio.Models.UserQuizScore;

public class AnswerDto
{
    public int QuestionId { get; set; }

    public List<int> Answers { get; set; } = [];

    public bool IsCorrect { get; set; }
}
