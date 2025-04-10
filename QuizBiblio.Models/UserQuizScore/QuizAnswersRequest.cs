namespace QuizBiblio.Models.UserQuizScore;

public class QuizAnswersRequest
{
    public required string QuizId { get; set; }

    public IEnumerable<AnswerRequest> Answers { get; set; } = [];
}