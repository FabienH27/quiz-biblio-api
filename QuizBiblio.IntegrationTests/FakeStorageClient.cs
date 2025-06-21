using Google.Api.Gax;
using Google.Apis.Upload;
using Google.Cloud.Storage.V1;
using QuizBiblio.Infrastructure.Storage;

namespace QuizBiblio.IntegrationTests;

public class FakeStorageClientWrapper : IStorageClientWrapper
{
    public Task CopyObjectAsync(string bucketName, string tempPath, string finalPath)
    {
        throw new NotImplementedException();
    }

    public Task DeleteObjectAsync(string bucketName, string objectName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteObjectAsync(Google.Apis.Storage.v1.Data.Object? image)
    {
        throw new NotImplementedException();
    }

    public Task<Page<Google.Apis.Storage.v1.Data.Object>> ListObjectsAsync(string bucketName, string location)
    {
        throw new NotImplementedException();
    }

    public Task<Google.Apis.Storage.v1.Data.Object> UploadObjectAsync(string bucket, string objectName, string contentType)
    {
        return Task.FromResult(new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = bucket,
            Name = objectName,
            ContentType = contentType,
        });
    }

    public Task UploadObjectAsync(string bucket, string objectName, string contentType, Stream stream)
    {
        throw new NotImplementedException();
    }
}
