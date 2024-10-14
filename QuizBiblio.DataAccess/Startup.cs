using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.DataAccess;
public static class Startup
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<BooksRepository>();
    }
}