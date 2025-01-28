using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public interface IQuizRepository
{
    public Task<List<QuizEntity>> GetQuizzesAsync();

    public Task<List<QuizInfo>> GetUserQuizzesAsync(string userId);

    public Task CreateQuiz(QuizEntity quiz);

    public Task UpdateQuiz(QuizEntity quiz);
}
