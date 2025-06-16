using Google.Cloud.SecretManager.V1;

namespace QuizBiblio.CloudStorage;

public class SecretManagerHelper
{
    public static string GetConnectionStringFromSecretManager(string secretName)
    {
        var projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT") ?? "xenon-timer-438013-s1";
        SecretManagerServiceClient secretManagerServiceClient = SecretManagerServiceClient.Create();
        var secretVersionName = new SecretVersionName(projectId, secretName, "latest");
        var response = secretManagerServiceClient.AccessSecretVersion(secretVersionName);
        return response.Payload.Data.ToStringUtf8();
    }
}
