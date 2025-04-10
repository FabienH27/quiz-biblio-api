using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.DataAccess.Theme;
using QuizBiblio.DataAccess.User;
using QuizBiblio.DataAccess.UserQuizScore;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.DataAccess.Guest;

namespace QuizBiblio.DataAccess;
public static class Startup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IGuestSessionRepository, GuestSessionRepository>();

        services.AddScoped<IThemeRepository, ThemeRepository>();

        services.AddScoped<IImageStorageRepository, ImageStorageRepository>();

        services.AddScoped<IUserQuizScoreRepository, UserQuizScoreRepository>();

        return services;
    }
}