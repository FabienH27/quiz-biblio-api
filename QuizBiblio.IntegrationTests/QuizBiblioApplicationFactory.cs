using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Moq;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Infrastructure.Configuration;
using QuizBiblio.Infrastructure.Storage;

namespace QuizBiblio.IntegrationTests;

internal class QuizBiblioApplicationFactory(string connectionString, string databaseName) : WebApplicationFactory<Program>
{
    private readonly string _connectionString = connectionString;
    private readonly string _databaseName = databaseName;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
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
            services.RemoveAll<IStorageClientWrapper>();
            services.RemoveAll<IDatabaseConfigurationProvider>();

            DisableRecurringJobs(services);

            services.AddSingleton<IDatabaseConfigurationProvider>(
                new FakeDatabaseConfigurationProvider(_connectionString, _databaseName));

            var mongoClientSettings = MongoClientSettings.FromConnectionString(_connectionString);
            mongoClientSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };

            var client = new MongoClient(mongoClientSettings);

            services.AddSingleton<IStorageClientWrapper, FakeStorageClientWrapper>();

            services.AddSingleton<IMongoClient>(client);
            
            //We add the actual implementation but with newly injected configuration
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
        });
    }

    private void DisableRecurringJobs(IServiceCollection services)
    {
        services.RemoveAll<IBackgroundJobClient>();
        services.RemoveAll<IRecurringJobManager>();
        services.RemoveAll<JobStorage>();
        services.RemoveAll<BackgroundJobServer>();
        var backgroundJobClientMock = new Mock<IBackgroundJobClient>();
        var jobStorageMock = new Mock<JobStorage>();
        services.AddSingleton(backgroundJobClientMock.Object);
        services.AddSingleton(jobStorageMock.Object);
    }
}
