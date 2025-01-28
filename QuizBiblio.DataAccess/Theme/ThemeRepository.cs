using Microsoft.EntityFrameworkCore;
using M = QuizBiblio.Models;

namespace QuizBiblio.DataAccess.Theme;

public class ThemeRepository(QuizBiblioDbContext dbContext) : IThemeRepository
{
    DbSet<Models.Theme> Themes => dbContext.Themes;

    public async Task<List<string>> GetThemesAsync() => await Themes.Select(x => x.Name).ToListAsync();

    public void CreateTheme(M.Theme theme)
    {
        Themes.Add(theme);
        dbContext.SaveChanges();
    }
}
