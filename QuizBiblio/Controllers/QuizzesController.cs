using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Services.Quiz;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Models.Quiz.Quiz>> GetQuizzes() => await quizService.GetQuizzesAsync();
}
