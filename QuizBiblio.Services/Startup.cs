using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess;
using QuizBiblio.Services.Quiz;

namespace QuizBiblio.Services;
public static class Startup
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRepositories();

        services.AddScoped<IQuizService, QuizService>();

        services.AddScoped<BooksService>();

        return services;
    }
}