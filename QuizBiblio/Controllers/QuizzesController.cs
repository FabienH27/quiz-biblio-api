using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services.Quiz;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Quiz>> GetQuizzes() => await quizService.GetQuizzesAsync();

    [HttpPost]
    public void CreateQuiz(Models.Quiz quiz) => quizService.CreateQuiz(quiz);
}
