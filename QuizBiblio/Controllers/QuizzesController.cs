using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Services.Quiz;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    public async Task<List<QuizEntity>> GetQuizzes() => await quizService.GetQuizzesAsync();

    //TODO: Make it admin accessible only
    [HttpGet("user")]
    public async Task<List<QuizEntity>> GetUserQuizzes()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return await quizService.GetUserQuizzesAsync(userId);
    }

    [HttpPost]
    public void CreateQuiz(QuizDto quiz) {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        var user = new QuizCreator
        {
            CreatorId = ObjectId.Parse(userId),
            Name = userName
        };

        quizService.CreateQuiz(quiz, user);
    }
}
