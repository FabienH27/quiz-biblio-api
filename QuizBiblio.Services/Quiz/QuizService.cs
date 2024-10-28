using QuizBiblio.DataAccess.Quiz;

namespace QuizBiblio.Services.Quiz;

public class QuizService(IQuizRepository quizRepository) : IQuizService
{

    public async Task<List<Models.Quiz>> GetQuizzesAsync() => await quizRepository.GetQuizzesAsync();

    public void CreateQuiz(Models.Quiz quiz)
    {
        quizRepository.CreateQuiz(quiz);
    }

}
