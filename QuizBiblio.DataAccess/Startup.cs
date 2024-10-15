using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess.Quiz;

namespace QuizBiblio.DataAccess;
public static class Startup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizRepository>();

        services.AddScoped<BooksRepository>();

        return services;
    }
}