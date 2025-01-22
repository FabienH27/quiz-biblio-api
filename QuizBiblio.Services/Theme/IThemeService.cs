using M = QuizBiblio.Models;

namespace QuizBiblio.Services.Theme;

public interface IThemeService
{
    /// <summary>
    /// Get all themes
    /// </summary>
    /// <returns>all themes</returns>
    public Task<List<string>> GetThemes();
}
