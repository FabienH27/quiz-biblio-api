using Google.Api.Gax;
using GAS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Services.CloudStorage;

public interface IStorageClientWrapper
{
    Task UploadObjectAsync(string bucket, string objectName, string contentType, Stream stream);

    Task CopyObjectAsync(string bucketName, string tempPath, string finalPath);

    Task<Page<GAS.Object>> ListObjectsAsync(string bucketName, string location);

    Task DeleteObjectAsync(string bucketName, string objectName);

    Task DeleteObjectAsync(GAS.Object? image);

}