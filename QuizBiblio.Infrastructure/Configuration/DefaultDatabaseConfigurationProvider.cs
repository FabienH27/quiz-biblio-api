using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;

namespace QuizBiblio.Infrastructure.Configuration;

public class DefaultDatabaseConfigurationProvider(IConfiguration configuration) : IDatabaseConfigurationProvider
{
    private readonly IConfiguration _configuration = configuration;

    private readonly SecretManagerServiceClient _client = SecretManagerServiceClient.Create();
    private readonly string _projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT") ?? "xenon-timer-438013-s1";

    public string? GetConnectionString()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        return env == "Development"
            ? _configuration.GetConnectionString("QuizStoreDatabase")
            : GetSecret("quiz-database-connection");
    }

    public string? GetDatabaseName()
    {
        return _configuration["QuizStoreDatabase:DatabaseName"];
    }

    private string GetSecret(string secretName)
    {
        var secretVersionName = new SecretVersionName(_projectId, secretName, "latest");
        var response = _client.AccessSecretVersion(secretVersionName);
        return response.Payload.Data.ToStringUtf8();
    }
}