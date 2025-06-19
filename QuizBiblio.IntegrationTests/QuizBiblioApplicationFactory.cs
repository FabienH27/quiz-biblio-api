using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;

namespace QuizBiblio.IntegrationTests;

internal class QuizBiblioApplicationFactory(string connectionString, string databaseName) : WebApplicationFactory<Program>
{
    private readonly string _connectionString = connectionString;
    private readonly string _databaseName = databaseName;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["QuizStoreDatabaseSettings:ConnectionString"] = _connectionString,
                ["QuizStoreDatabaseSettings:DatabaseName"] = _databaseName
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IMongoClient>();
            services.RemoveAll<IMongoDbContext>();

            var mongoClientSettings = MongoClientSettings.FromConnectionString(_connectionString);
            mongoClientSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            var client = new MongoClient(mongoClientSettings);

            services.AddSingleton<IMongoClient>(client);
            
            //We add the actual implementation but with newly injected configuration
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
        });
    }

}
