using QuizBiblio.DataAccess.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Services.Theme;

public class ThemeService(IThemeRepository themeRepository) : IThemeService
{
    public async Task<List<string>> GetThemes() => await themeRepository.GetThemesAsync();
}
