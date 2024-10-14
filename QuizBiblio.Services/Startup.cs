using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Services;
public static class Startup
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //services.AddRepositories();

        services.AddSingleton<BooksService>();

        return services;
    }
}