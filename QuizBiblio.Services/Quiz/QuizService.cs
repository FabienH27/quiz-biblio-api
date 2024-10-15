using QuizBiblio.DataAccess.Quiz;
namespace QuizBiblio.Services.Quiz;

public class QuizService(IQuizRepository quizRepository) : IQuizService
{
    public async Task<List<Models.Quiz.Quiz>> GetQuizzesAsync() => await quizRepository.GetQuizzesAsync();

}
