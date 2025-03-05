using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;
using GS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.DataAccess.ImageStorage;

public class ImageStorageRepository : IImageStorageRepository
{

    private readonly StorageClient _storageClient;
    private readonly BucketSettings _bucketSettings;

    public ImageStorageRepository(StorageClient storageClient, IOptions<BucketSettings> bucketSettings)
    {
        _storageClient = storageClient;
        _bucketSettings = bucketSettings.Value;
    }

    public async Task<UploadResult> UploadImageAsync(IFormFile file, string contentType)
    {
        using var stream = file.OpenReadStream();

        var uniqueFileName = Guid.NewGuid();
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        var fileName = $"uploads/{uniqueFileName}{fileExtension}";

        var uploadResponse = await _storageClient.UploadObjectAsync(
            _bucketSettings.Name,
            fileName,
            contentType,
            stream
            );
        
        return new UploadResult
        {
            ImageUrl = $"https://storage.googleapis.com/{uploadResponse.Bucket}/{fileName}"
        };
    }
}
