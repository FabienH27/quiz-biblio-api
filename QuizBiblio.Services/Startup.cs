using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess;
using QuizBiblio.Services.Quiz;
using QuizBiblio.Services.Theme;
using QuizBiblio.Services.User;

namespace QuizBiblio.Services;
public static class Startup
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRepositories();

        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IThemeService, ThemeService>();
       
        services.AddScoped<BooksService>();

        return services;
    }
}