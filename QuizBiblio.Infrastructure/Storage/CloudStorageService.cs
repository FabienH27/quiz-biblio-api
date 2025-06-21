using Google;
using Google.Api.Gax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;
using GAS = Google.Apis.Storage.v1.Data;


namespace QuizBiblio.Infrastructure.Storage;

public class CloudStorageService : ICloudStorageService
{
    private readonly ILogger<CloudStorageService> _logger;

    private readonly BucketSettings _bucketSettings;

    private readonly string _assetsLocation;
    private readonly string _bucketName;
    private readonly string _tempLocation;

    private readonly IStorageClientWrapper _client;

    public CloudStorageService(IOptions<BucketSettings> bucketSettings, IStorageClientWrapper client, ILogger<CloudStorageService> logger)
    {
        _bucketSettings = bucketSettings.Value;
        _assetsLocation = _bucketSettings.QuizImageAssetsLocation;
        _bucketName = _bucketSettings.Name;
        _tempLocation = _bucketSettings.TemporaryImageLocation;
        _client = client;
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(Stream stream, string fileName, string contentType)
    {
        var storageLocation = $"{_tempLocation}/{fileName}";

        await _client.UploadObjectAsync(
            _bucketName,
            storageLocation,
            contentType,
            stream
        );

        return storageLocation;
    }

    public async Task<string> MoveImageToAssetsAsync(ImageEntity image)
    {
        string tempPath = $"{_tempLocation}/{image.Id}";
        string finalPath = $"{_assetsLocation}/{image.Id}";

        await _client.CopyObjectAsync(_bucketName, tempPath, finalPath);

        await _client.DeleteObjectAsync(_bucketName, tempPath);

        return finalPath;
    }

    public async Task<Page<GAS.Object>> ListImages() => await _client.ListObjectsAsync(_bucketName, _tempLocation);

    public async Task<bool> DeleteTemporaryImages()
    {
        var images = await ListImages();

        var deletedImages = new List<string>();

        foreach (var image in images)
        {
            try
            {
                await _client.DeleteObjectAsync(image);
                deletedImages.Add(image.Id);
            }catch(GoogleApiException apiException)
            {
                _logger.LogError("Failure while deleting image on Cloud Storage : {apiException}", apiException);
            }
        }

        return deletedImages.Count == images.Count();
    }
}
