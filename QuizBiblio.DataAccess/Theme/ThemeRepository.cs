using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using M = QuizBiblio.Models;

namespace QuizBiblio.DataAccess.Theme;

public class ThemeRepository : IThemeRepository
{
    private readonly IMongoDbContext _dbContext;

    IMongoCollection<M.Theme> Themes => _dbContext.GetCollection<M.Theme>("Themes");

    public ThemeRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Creates a theme
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    public async Task CreateTheme(M.Theme theme)
    {
        await Themes.InsertOneAsync(theme);
    }

    /// <summary>
    /// Gets all themes
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetThemesAsync()
    {
        var projection = Builders<M.Theme>.Projection.Expression(t => t.Name);
        return await Themes.Find(_ => true).Project(projection).ToListAsync();
    }
}
