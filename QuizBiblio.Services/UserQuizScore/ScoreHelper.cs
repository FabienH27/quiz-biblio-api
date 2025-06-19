using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Services.UserQuizScore;

public static class ScoreHelper
{
    /// <summary>
    /// Calculates score from answers for a quiz
    /// </summary>
    /// <param name="answers">answers gave by the user</param>
    /// <returns>computed new score</returns>
    public static int CalculateUserScore(IEnumerable<AnswerDto> answers) => answers.Where(x => x.IsCorrect).Count();
}
