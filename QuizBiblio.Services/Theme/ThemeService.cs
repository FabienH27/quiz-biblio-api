using QuizBiblio.DataAccess.Theme;
using M = QuizBiblio.Models; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Services.Theme;

public class ThemeService(IThemeRepository themeRepository) : IThemeService
{
    public async Task<List<string>> GetThemesAsync() => await themeRepository.GetThemesAsync();

    public void CreateTheme(M.Theme theme) => themeRepository.CreateTheme(theme);
}
