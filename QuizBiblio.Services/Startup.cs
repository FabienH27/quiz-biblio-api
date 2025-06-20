using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess;
using QuizBiblio.Services.CloudStorage;
using QuizBiblio.Services.Guest;
using QuizBiblio.Services.ImageStorage;
using QuizBiblio.Services.Quiz;
using QuizBiblio.Services.Theme;
using QuizBiblio.Services.User;
using QuizBiblio.Services.UserQuizScore;

namespace QuizBiblio.Services;
public static class Startup
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRepositories();

        services.AddScoped<IQuizService, QuizService>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IGuestSessionService, GuestSessionService>();
        
        services.AddScoped<IThemeService, ThemeService>();

        services.AddScoped<ICloudStorageService, CloudStorageService>();

        services.AddScoped<IImageStorageService, ImageStorageService>();
        
        services.AddScoped<IUserQuizScoreService, UserQuizScoreService>();
       
        return services;
    }
}