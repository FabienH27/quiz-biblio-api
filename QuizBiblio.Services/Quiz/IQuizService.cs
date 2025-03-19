using QuizBiblio.Models.Quiz;

namespace QuizBiblio.Services.Quiz;

public interface IQuizService
{
    public Task<List<QuizInfo>> GetQuizzesAsync();

    public Task<QuizDto> GetByIdAsync(string quizId);

    public Task<List<QuizInfo>> GetUserQuizzesAsync(string userId);

    public Task CreateQuiz(QuizDto quiz);

    public Task UpdateQuiz(QuizDto quizDto);

    public Task DeleteQuizAsync(string quizId);
}
