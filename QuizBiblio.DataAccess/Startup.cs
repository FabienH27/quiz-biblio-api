﻿using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess.Quiz;
using QuizBiblio.DataAccess.User;

namespace QuizBiblio.DataAccess;
public static class Startup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<BooksRepository>();

        return services;
    }
}