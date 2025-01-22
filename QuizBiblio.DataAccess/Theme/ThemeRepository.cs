using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.DataAccess.Theme;

public class ThemeRepository(QuizBiblioDbContext dbContext) : IThemeRepository
{
    DbSet<Models.Theme> Themes => dbContext.Themes;

    public async Task<List<string>> GetThemesAsync() => await Themes.Select(x => x.Name).ToListAsync();
}
