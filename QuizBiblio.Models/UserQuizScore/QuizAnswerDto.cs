namespace QuizBiblio.Models.UserQuizScore;

public class QuizAnswerDto
{
    public required string QuizId { get; set; }

    public IEnumerable<AnswerDto> Answers { get; set; } = [];
}
