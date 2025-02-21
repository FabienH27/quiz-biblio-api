using MongoDB.Bson;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Services.Utils;

namespace QuizBiblio.Services.Quiz;

public class QuizService(IQuizRepository quizRepository) : IQuizService
{
    /// <summary>
    /// Gets all quizzes
    /// </summary>
    /// <returns>all quizzes. Not yet paginated</returns>
    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await quizRepository.GetQuizzesAsync();
    }

    /// <summary>
    /// Get quiz by id
    /// </summary>
    /// <param name="quizId">id of the quiz</param>
    /// <returns></returns>
    public async Task<QuizDto> GetByIdAsync(string quizId) => await quizRepository.GetQuiz(quizId);

    /// <summary>
    /// Gets quizzes created by a specific user
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <returns>list of quiz information</returns>
    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId) => await quizRepository.GetUserQuizzesAsync(userId);
   
    /// <summary>
    /// Creates a new quiz
    /// </summary>
    /// <param name="quiz">quiz to create</param>
    public async Task CreateQuiz(QuizDto quiz)
    {
        await quizRepository.CreateQuiz(quiz.ToEntity());
    }

    /// <summary>
    /// Updates a quiz
    /// </summary>
    /// <param name="quiz">quiz to update</param>
    /// <returns></returns>
    public async Task UpdateQuiz(QuizDto quiz)
    {
        await quizRepository.UpdateQuiz(quiz.ToEntity());
    }
}
