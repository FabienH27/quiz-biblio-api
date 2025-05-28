using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Helper;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Requests;
using QuizBiblio.Services.Quiz;
using QuizBiblio.Services.Utils;

namespace QuizBiblio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService, QuizMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all quizzes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<QuizInfo>> GetQuizzes() => await quizService.GetQuizzesAsync();

    /// <summary>
    /// Gets quiz by id
    /// Users does not need to authenticate to play quizzes : they can choose to authenticate after
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
    [HttpGet("{quizId}")]
    [AllowAnonymous]
    public async Task<QuizDto> GetQuiz(string quizId)
    {
        return await quizService.GetByIdAsync(quizId);
    }

    //TODO: Make it admin accessible only
    /// <summary>
    /// Get quizzes for a specific user
    /// </summary>
    /// <returns></returns>
    [HttpGet("user")]
    public async Task<List<QuizInfo>> GetUserQuizzes()
    {
        var userId = User.Claims.GetUserId();
        return await quizService.GetUserQuizzesAsync(userId);
    }

    /// <summary>
    /// Creates a quiz
    /// </summary>
    /// <param name="quiz"></param>
    [HttpPost]
    public async Task CreateQuiz(CreateQuizRequest request) {
        var userId = User.Claims.GetUserId();
        var userName = User.Claims.GetUserName();
        var user = new QuizCreator
        {
            Id = userId,
            Name = userName
        };

        var quizDto = mapper.Map(request, user);

        await quizService.CreateQuiz(quizDto);
    }

    [HttpPut("{quizId}")]
    public async Task<IActionResult> UpdateQuiz([FromRoute] string quizId, [FromBody] UpdateQuizRequest quiz)
    {
        var existingQuiz = await quizService.GetByIdAsync(quizId);
        if(existingQuiz == null)
        {
            return NotFound();
        }

        //await quizService.UpdateQuiz(quiz);

        return NoContent();
    }

    [HttpDelete("{quizId}")]
    public async Task<IActionResult> DeleteQuiz([FromRoute] string quizId)
    {
        await quizService.DeleteQuizAsync(quizId);

        return NoContent();
    }
}
