using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.Infrastructure.Configuration;

namespace QuizBiblio.JobScheduler;

public static class Startup
{
    public static IServiceCollection AddJobScheduler(this IServiceCollection services)
    {
        services.AddHangfire((provider, config) =>
        {
            var configurationProvider = provider.GetRequiredService<IDatabaseConfigurationProvider>();

            var connectionString = configurationProvider.GetConnectionString();
            var dbName = configurationProvider.GetDatabaseName();

            var options = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new DropMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                },
                CheckConnection = true
            };

            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseMongoStorage(connectionString, dbName, options);
        });

        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = "Hangfire.Mongo Server 1";
        });

        return services;

    }
}
