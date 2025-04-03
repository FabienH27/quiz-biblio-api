using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.DependencyInjection;

namespace QuizBiblio.JobScheduler;

public static class Startup
{
    public static IServiceCollection AddJobScheduler(this IServiceCollection services, string connectionString)
    {
        var options = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new NoneMongoBackupStrategy()
            },
            CheckConnection = true
        };

        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(connectionString, options)
        );

        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = "Hangfire.Mongo Server 1";
        });

        return services;
    }
}
