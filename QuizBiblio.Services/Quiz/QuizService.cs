using MongoDB.Bson;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Utils;

namespace QuizBiblio.Services.Quiz;

public class QuizService(IQuizRepository quizRepository) : IQuizService
{
    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await quizRepository.GetQuizzesAsync();
    }

    public async Task<QuizDto> GetByIdAsync(string quizId) => await quizRepository.GetQuiz(quizId);

    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId) => await quizRepository.GetUserQuizzesAsync(userId);
   
    public void CreateQuiz(QuizDto quiz)
    {
        var newQuiz = new QuizEntity
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Creator = quiz.Creator,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes
        };

        quizRepository.CreateQuiz(newQuiz);
    }

    public async Task UpdateQuiz(QuizDto quiz)
    {
        var quizEntity = new QuizEntity
        {
            Id = quiz.Id,
            Title = quiz.Title,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes,
            Creator = quiz.Creator,
        };

        await quizRepository.UpdateQuiz(quizEntity);
    }
}
