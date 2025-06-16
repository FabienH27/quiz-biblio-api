using Microsoft.Extensions.Options;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services.Exceptions;
using QuizBiblio.Services.ImageStorage;
using QuizBiblio.Services.Utils;

namespace QuizBiblio.Services.Quiz;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IImageStorageService _imageStorageService;

    public QuizService(IQuizRepository quizRepository, IImageStorageService imageStorageService)
    {
        _quizRepository = quizRepository;
        _imageStorageService = imageStorageService;
    }

    /// <summary>
    /// Gets all quizzes
    /// </summary>
    /// <returns>all quizzes. Not yet paginated</returns>
    public async Task<List<QuizInfo>> GetQuizzesAsync()
    {
        return await _quizRepository.GetQuizzesAsync();
    }

    /// <summary>
    /// Get quiz by id
    /// </summary>
    /// <param name="quizId">id of the quiz</param>
    /// <returns></returns>
    public async Task<QuizDto> GetByIdAsync(string quizId) => await _quizRepository.GetQuiz(quizId);

    /// <summary>
    /// Gets quizzes created by a specific user
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <returns>list of quiz information</returns>
    public async Task<List<QuizInfo>> GetUserQuizzesAsync(string userId) => await _quizRepository.GetUserQuizzesAsync(userId);
   
    /// <summary>
    /// Creates a new quiz
    /// </summary>
    /// <param name="quiz">quiz to create</param>
    public async Task CreateQuiz(QuizDto quiz)
    {
        if (!string.IsNullOrEmpty(quiz.ImageId))
        {
            await SaveImageToAssets(quiz.ImageId);
        }

        await _quizRepository.CreateQuiz(quiz.ToEntity());
    }

    /// <summary>
    /// Updates a quiz
    /// </summary>
    /// <param name="quiz">quiz to update</param>
    /// <returns></returns>
    public async Task UpdateQuiz(QuizDto quiz)
    {
        if (!string.IsNullOrEmpty(quiz.ImageId))
        {
            await SaveImageToAssets(quiz.ImageId);
        }

        var questionImages = quiz.Questions
            .Select(x => x.ImageId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id!);

        var tasks = questionImages.Select(SaveImageToAssets);
        await Task.WhenAll(tasks);

        await _quizRepository.UpdateQuiz(quiz.ToEntity());
    }

    /// <summary>
    /// Moves the image to assets
    /// </summary>
    /// <param name="imageId">id of the image to move</param>
    /// <returns></returns>
    public async Task SaveImageToAssets(string imageId)
    {
        await _imageStorageService.MoveImageToAssetsAsync(imageId);

    }

    public async Task DeleteQuizAsync(string quizId)
    {
        await _quizRepository.DeleteQuizAsync(quizId);
    }
}
