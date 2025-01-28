using QuizBiblio.Models.Quiz;

namespace QuizBiblio.Services.Quiz;

public interface IQuizService
{
    public Task<List<QuizEntity>> GetQuizzesAsync();

    public Task<List<QuizEntity>> GetUserQuizzesAsync(string userId);

    public void CreateQuiz(QuizDto quiz, QuizCreator quizCreator);
}
