using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public interface IQuizRepository
{
    public Task<List<QuizEntity>> GetQuizzesAsync();

    public Task<List<QuizEntity>> GetUserQuizzesAsync(string userId);

    public void CreateQuiz(QuizEntity quiz);

    public void UpdateQuiz(QuizEntity quiz);
}
