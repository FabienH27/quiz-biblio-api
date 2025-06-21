using Google.Api.Gax;
using Google.Cloud.Storage.V1;
using GAS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Infrastructure.Storage;

public class StorageClientWrapper : IStorageClientWrapper
{
    private readonly StorageClient _client;

    public StorageClientWrapper()
    {
        _client = StorageClient.Create();
    }

    public async Task UploadObjectAsync(string bucket, string objectName, string contentType, Stream stream)
    {
        await _client.UploadObjectAsync(bucket, objectName, contentType, stream);
    }

    public async Task CopyObjectAsync(string bucketName, string tempPath, string finalPath)
    {
        await _client.CopyObjectAsync(bucketName, tempPath, bucketName, finalPath);
    }

    public async Task<Page<GAS.Object>> ListObjectsAsync(string bucketName, string location) => await _client.ListObjectsAsync(bucketName, location).ReadPageAsync(50);

    public async Task DeleteObjectAsync(string bucketName, string objectName)
    {
        await _client.DeleteObjectAsync(bucketName, objectName);
    }

    public async Task DeleteObjectAsync(GAS.Object? image)
    {
        await _client.DeleteObjectAsync(image);
    }

}
