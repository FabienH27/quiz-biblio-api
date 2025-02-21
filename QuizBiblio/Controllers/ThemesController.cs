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
    /// <summary>
    /// Gets all themes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<string>> GetThemes() => await themeService.GetThemesAsync();


    /// <summary>
    /// Creates a theme
    /// </summary>
    /// <param name="theme"></param>
    [HttpPost]
    public void CreateTheme([FromBody] Theme theme) => themeService.CreateTheme(theme);
}
