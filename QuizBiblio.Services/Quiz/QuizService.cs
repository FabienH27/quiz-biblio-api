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
    private readonly BucketSettings _bucketSettings;

    public QuizService(IQuizRepository quizRepository, IImageStorageService imageStorageService, IOptions<BucketSettings> bucketSettings)
    {
        _quizRepository = quizRepository;
        _imageStorageService = imageStorageService;
        _bucketSettings = bucketSettings.Value;
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
        quiz.Image = await UpdateImageLocation(quiz);

        await _quizRepository.CreateQuiz(quiz.ToEntity());
    }

    /// <summary>
    /// Updates a quiz
    /// </summary>
    /// <param name="quiz">quiz to update</param>
    /// <returns></returns>
    public async Task UpdateQuiz(QuizDto quiz)
    {
        quiz.Image = await UpdateImageLocation(quiz);

        await _quizRepository.UpdateQuiz(quiz.ToEntity());
    }

    public async Task<string> UpdateImageLocation(QuizDto quiz)
    {
        if (!string.IsNullOrEmpty(quiz.Image) && quiz.Image.Contains(_bucketSettings.TemporaryImageLocation))
        {
            var newImageUrl = await _imageStorageService.MoveImageToAssetsAsync(quiz.Image);

            //if moving the image did not succeed : set empty string
            quiz.Image = string.IsNullOrEmpty(newImageUrl) ? string.Empty : newImageUrl;
        }

        return quiz.Image ?? string.Empty;
    }
}
