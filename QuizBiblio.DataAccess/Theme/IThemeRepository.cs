using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = QuizBiblio.Models;

namespace QuizBiblio.DataAccess.Theme;

public interface IThemeRepository
{
    public Task<List<string>> GetThemesAsync();

    public void CreateTheme(M.Theme theme);
}
