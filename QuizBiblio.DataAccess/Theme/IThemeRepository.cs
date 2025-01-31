using M = QuizBiblio.Models;

namespace QuizBiblio.DataAccess.Theme;

public interface IThemeRepository
{
    public Task<List<string>> GetThemesAsync();

    public Task CreateTheme(M.Theme theme);
}
