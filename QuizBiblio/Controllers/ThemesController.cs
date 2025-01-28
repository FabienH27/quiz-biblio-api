using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services.Quiz;
using QuizBiblio.Services.Theme;

namespace QuizBiblio.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ThemesController(IThemeService themeService) : ControllerBase
{
    [HttpGet]
    public async Task<List<string>> GetThemes() => await themeService.GetThemesAsync();


    [HttpPost]
    public void CreateTheme([FromBody] Theme theme) => themeService.CreateTheme(theme);
}
