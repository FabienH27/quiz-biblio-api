using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using QuizBiblio.Helper;
using QuizBiblio.Models.DatabaseSettings;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Utils;
using QuizBiblio.Services.Quiz;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<QuizInfo>> GetQuizzes() => await quizService.GetQuizzesAsync();

    [HttpGet("{quizId}")]
    public async Task<QuizDto> GetQuiz(string quizId)
    {
        return await quizService.GetByIdAsync(quizId);
    }

    //TODO: Make it admin accessible only
    [HttpGet("user")]
    public async Task<List<QuizInfo>> GetUserQuizzes()
    {
        var userId = User.Claims.GetUserId();
        return await quizService.GetUserQuizzesAsync(userId);
    }

    [HttpPost]
    public void CreateQuiz(CreateQuizResponse quiz) {
        var userId = User.Claims.GetUserId();
        var userName = User.Claims.GetUserName();
        var user = new QuizCreator
        {
            Id = userId,
            Name = userName
        };

        var quizDto = new QuizDto
        {
            Title = quiz.Title,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes,
            Creator = user
        };

        quizService.CreateQuiz(quizDto);
    }

    [HttpPut("{quizId}")]
    public async Task<IActionResult> UpdateQuiz([FromRoute] string quizId, [FromBody] QuizDto quiz)
    {
        var existingQuiz = await quizService.GetByIdAsync(quizId);
        if(existingQuiz == null)
        {
            return NotFound();
        }

        await quizService.UpdateQuiz(quiz);

        return NoContent();
    }
}
