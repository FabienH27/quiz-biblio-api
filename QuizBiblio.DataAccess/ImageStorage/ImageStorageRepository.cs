using Google;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;

namespace QuizBiblio.DataAccess.ImageStorage;

public class ImageStorageRepository : IImageStorageRepository
{

    private readonly ILogger<ImageStorageRepository> _logger;

    private readonly StorageClient _storageClient;
    private readonly BucketSettings _bucketSettings;

    private readonly string _tempLocation;
    private readonly string _assetsLocation;
    private readonly string _bucketName;

    public ImageStorageRepository(ILogger<ImageStorageRepository> logger,
        StorageClient storageClient, IOptions<BucketSettings> bucketSettings)
    {
        _logger = logger;
        
        _storageClient = storageClient;
        _bucketSettings = bucketSettings.Value;
        
        _bucketName = _bucketSettings.Name;
        _tempLocation = _bucketSettings.TemporaryImageLocation;
        _assetsLocation = _bucketSettings.QuizImageAssetsLocation;
    }

    public async Task<UploadResult> UploadImageAsync(IFormFile file, string contentType)
    {
        using var stream = file.OpenReadStream();

        var uniqueFileName = Guid.NewGuid();
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        var fileName = $"{uniqueFileName}{fileExtension}";

        var storageLocation = $"{_tempLocation}/{fileName}";

        var uploadResponse = await _storageClient.UploadObjectAsync(
            _bucketName,
            storageLocation,
            contentType,
            stream
            );

        return new UploadResult
        {
            ImageUrl = storageLocation
        };
    }


    public async Task<string> MoveImageToAssets(string imageUrl)
    {
        var splitUrl = imageUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string imageName = splitUrl.Last();

        string tempPath = $"{_tempLocation}/{imageName}";
        string finalPath = $"{_assetsLocation}/{imageName}";

        try
        {
            var response = await _storageClient.CopyObjectAsync(
                _bucketName, tempPath,
                _bucketName, finalPath
            );

            await _storageClient.DeleteObjectAsync(_bucketName, tempPath);
            return finalPath;
        }catch(GoogleApiException ex)
        {
            _logger.LogError("Could not move image : {Message}", ex.Message);
            return string.Empty;
        }

    }
}
