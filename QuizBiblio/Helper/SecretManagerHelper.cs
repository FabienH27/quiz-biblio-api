using Google.Cloud.SecretManager.V1;

namespace QuizBiblio.Helper;

public class SecretManagerHelper
{
    public static string GetConnectionStringFromSecretManage(string secretName)
    {
        SecretManagerServiceClient secretManagerServiceClient = SecretManagerServiceClient.Create();
        var secretVersionName = new SecretVersionName("xenon-timer-438013-s1", secretName, "latest");
        var response = secretManagerServiceClient.AccessSecretVersion(secretVersionName);
        return response.Payload.Data.ToStringUtf8().Trim();
    }
}
