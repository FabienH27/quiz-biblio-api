using Google;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.DataAccess.Utils;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;

namespace QuizBiblio.DataAccess.ImageStorage;

public class ImageStorageRepository : IImageStorageRepository
{

    private readonly ILogger<ImageStorageRepository> _logger;
    private readonly IMongoDbContext _dbContext;

    private readonly StorageClient _storageClient;
    private readonly BucketSettings _bucketSettings;

    private readonly string _tempLocation;
    private readonly string _assetsLocation;
    private readonly string _bucketName;

    IMongoCollection<ImageEntity> Images => _dbContext.GetCollection<ImageEntity>("Images");

    public ImageStorageRepository(ILogger<ImageStorageRepository> logger,
        IMongoDbContext dbContext,
        StorageClient storageClient, IOptions<BucketSettings> bucketSettings)
    {
        _logger = logger;

        _dbContext = dbContext;
        
        _storageClient = storageClient;
        _bucketSettings = bucketSettings.Value;
        
        _bucketName = _bucketSettings.Name;
        _tempLocation = _bucketSettings.TemporaryImageLocation;
        _assetsLocation = _bucketSettings.QuizImageAssetsLocation;
    }

    /// <summary>
    /// Uploads the image on the bucket and creates an entry on the MongoDB collection
    /// </summary>
    /// <param name="file">image to upload to bucket</param>
    /// <param name="contentType">content type of the image</param>
    /// <returns>an object containing the id of the new image</returns>
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

        var imageEntity = new ImageEntity
        {
            OriginalUrl = storageLocation,
            CreatedAt = DateTime.UtcNow
        };

        await Images.InsertOneAsync(imageEntity);

        return new UploadResult
        {
            ImageId = imageEntity.Id,
            ImageUrl = storageLocation
        };
    }

    public async Task MoveImageToAssets(string imageId)
    {
        var image = await GetImageAsync(imageId);

        if(image != null)
        {
            var splitUrl = image.OriginalUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string imageName = splitUrl.Last();

            string tempPath = $"{_tempLocation}/{imageName}";
            string finalPath = $"{_assetsLocation}/{imageName}";
            string resizedImageUrl = ImageHelper.GetResizedUrl(finalPath, _bucketSettings.ResizedImageWidth);

            try
            {
                var response = await _storageClient.CopyObjectAsync(
                    _bucketName, tempPath,
                    _bucketName, finalPath
                );

                await _storageClient.DeleteObjectAsync(_bucketName, tempPath);

                var filter = Builders<ImageEntity>.Filter.Eq(img => img.Id, image.Id);
                var update = Builders<ImageEntity>.Update
                    .Set(img => img.OriginalUrl, finalPath)
                    .Set(img => img.ResizedUrl, resizedImageUrl)
                    .Set(img => img.IsPermanent, true);

                await Images.FindOneAndUpdateAsync(filter, update);

            }catch(GoogleApiException ex)
            {
                _logger.LogError("Could not move image : {Message}", ex.Message);
            }
        }
        else
        {
            _logger.LogError("Could not move image.");
        }
    }

    /// <summary>
    /// Gets an image from an image id
    /// </summary>
    /// <param name="imageId">id of the document</param>
    /// <returns></returns>
    public async Task<ImageEntity> GetImageAsync(string imageId)
    {
        return await Images.Find(image => image.Id == imageId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Delete images stored in the temporary asset folder
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteTemporaryImages()
    {
        var result = _storageClient.ListObjectsAsync(_bucketName, _tempLocation);

        var data = await result.ReadPageAsync(50);

        var deletedImages = new List<string>();

        foreach(var image in data)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(image);
                deletedImages.Add(image.Id);
            }catch(GoogleApiException apiException)
            {
                _logger.LogError("Failure while deleting image on Cloud Storage : {apiException}", apiException);
            }           
        }

        //Only delete image documents if deleted image count match the initial image count to delete
        if(deletedImages.Count == data.Count())
        {
            //Deletes images if they are marked as permanent OR folder points out to the temporary folder
            await Images.DeleteManyAsync(image => image.IsPermanent == false || image.OriginalUrl.StartsWith(_tempLocation));
            return true;
        }
        else
        {
            _logger.LogWarning("Did not delete documents as an error occured while deleting images in Cloud Storage");
        }

        return false;
    }
}
