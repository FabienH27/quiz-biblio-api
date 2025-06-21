using Google;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;

namespace QuizBiblio.DataAccess.ImageStorage;

public class ImageStorageRepository : IImageStorageRepository
{
    private readonly ILogger<ImageStorageRepository> _logger;
    private readonly IMongoDbContext _dbContext;

    private readonly BucketSettings _bucketSettings;

    private readonly string _tempLocation;

    IMongoCollection<ImageEntity> Images => _dbContext.GetCollection<ImageEntity>("Images");

    public ImageStorageRepository(ILogger<ImageStorageRepository> logger,
        IMongoDbContext dbContext,
        IOptions<BucketSettings> bucketSettings)
    {
        _logger = logger;

        _dbContext = dbContext;
        
        //_storageClient = storageClient;
        _bucketSettings = bucketSettings.Value;
        _tempLocation = _bucketSettings.TemporaryImageLocation;
    }

    /// <summary>
    /// Creates an entry on the db for a given storage location
    /// </summary>
    /// <param name="storageLocation">location of the stored image</param>
    /// <returns>an object containing the id of the new image</returns>
    public async Task<UploadResult> SaveImageAsync(string storageLocation)
    {
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

    public async Task MoveImageToAssetsAsync(ImageEntity image)
    {
        try
        {
            var filter = Builders<ImageEntity>.Filter.Eq(img => img.Id, image.Id);
            var update = Builders<ImageEntity>.Update
                .Set(img => img.OriginalUrl, image.OriginalUrl)
                .Set(img => img.ResizedUrl, image.ResizedUrl)
                .Set(img => img.IsPermanent, true);

            await Images.FindOneAndUpdateAsync(filter, update);

        }catch(GoogleApiException ex)
        {
            _logger.LogError("Could not move image : {Message}", ex.Message);
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
    public async Task DeleteTemporaryImages()
    {
        //Deletes images if they are marked as permanent (meaning: moved to assets) OR folder targeting the temporary folder
        await Images.DeleteManyAsync(image => image.IsPermanent == false || image.OriginalUrl.StartsWith(_tempLocation));
    }
}
