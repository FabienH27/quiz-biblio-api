using MongoDB.Bson;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.Services.Quiz;

public class QuizService(IQuizRepository quizRepository) : IQuizService
{
    public async Task<List<QuizEntity>> GetQuizzesAsync() => await quizRepository.GetQuizzesAsync();

    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId) => await quizRepository.GetUserQuizzesAsync(userId);
   
    public void CreateQuiz(QuizDto quiz, QuizCreator quizCreator)
    {
        var newQuiz = new QuizEntity
        {
            Title = quiz.Title,
            Creator = quizCreator,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes
        };

        quizRepository.CreateQuiz(newQuiz);
    }
}
