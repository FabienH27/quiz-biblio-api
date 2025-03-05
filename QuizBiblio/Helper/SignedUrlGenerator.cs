using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Settings;

namespace QuizBiblio.Helper;

public class SignedUrlGenerator(IOptions<BucketSettings> bucketSettings)
{
    private readonly BucketSettings _bucketSettings = bucketSettings.Value;

    public string GenerateSignedUrl(string objectName)
    {
        var googleCredentials = GoogleCredential.GetApplicationDefault();

        var signer = UrlSigner.FromCredential(googleCredentials);

        var expiration = TimeSpan.FromMinutes(15);

        string signedUrl = signer.Sign(_bucketSettings.Name, objectName, expiration);

        return signedUrl;
    }
}
