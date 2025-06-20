using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.DataAccess.Utils;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services.CloudStorage;
using QuizBiblio.Services.Exceptions;

namespace QuizBiblio.Services.ImageStorage;

public class ImageStorageService : IImageStorageService
{
    private readonly IImageStorageRepository _imageStorageRepository;

    private readonly ICloudStorageService _cloudStorageService;
    
    private readonly BucketSettings _bucketSettings;

    public ImageStorageService(IImageStorageRepository imageStorageRepository, ICloudStorageService cloudStorageService, IOptions<BucketSettings> bucketSettings)
    {
        _imageStorageRepository = imageStorageRepository;
        _cloudStorageService = cloudStorageService;
        
        _bucketSettings = bucketSettings.Value;
    }

    /// <summary>
    /// Uploads an image to temporary folder
    /// </summary>
    /// <param name="file">fiile data to upload</param>
    /// <returns>image id and url</returns>
    /// <exception cref="ImageUploadException">exception thrown in case of invalid data</exception>
    public async Task<UploadResult> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ImageUploadException("No file uploaded.");
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(file.FileName, out var contentType))
        {
            throw new ImageUploadException("Could not determine file type.");
        }

        var allowedExtensions = new HashSet<string> { ".png", ".jpg", ".jpeg", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension) || !contentType.StartsWith("image/"))
        {
            throw new ImageUploadException("Invalid file type. Only images are allowed.");
        }

        if (file.Length > 5 * 1024 * 1024)
        {
            throw new ImageUploadException("File exceeds the 5MB limit.");
        }

        using var stream = file.OpenReadStream();

        var uniqueFileName = Guid.NewGuid();
        var fileName = $"{uniqueFileName}{fileExtension}";

        var storageLocation = await _cloudStorageService.SaveFileAsync(stream, fileName, contentType);

        return await _imageStorageRepository.SaveImageAsync(storageLocation);
    }

    /// <summary>
    /// Moves image to asset once the user assigned it to a quiz
    /// </summary>
    /// <param name="imageId">id of the image to move</param>
    /// <returns></returns>
    public async Task MoveImageToAssetsAsync(string imageId)
    {
        var image = await _imageStorageRepository.GetImageAsync(imageId);

        var imagePath = await _cloudStorageService.MoveImageToAssetsAsync(image);

        string resizedImageUrl = ImageHelper.GetResizedUrl(imagePath, _bucketSettings.ResizedImageWidth);

        image.ResizedUrl = resizedImageUrl;
        image.OriginalUrl = imagePath;

        await _imageStorageRepository.MoveImageToAssetsAsync(image);
    }

    /// <summary>
    /// Gets an image from image id
    /// </summary>
    /// <param name="imageId">id of the document</param>
    /// <returns></returns>
    public async Task<ImageDto> GetImageAsync(string imageId)
    {
        var result = await _imageStorageRepository.GetImageAsync(imageId);

        return new ImageDto
        {
            OriginalUrl = result.OriginalUrl,
            ResizedUrl = result.ResizedUrl,
            IsPermanent = result.IsPermanent,
        };
    }

    /// <summary>
    /// Deletes temporary images
    /// </summary>
    public async Task DeleteTemporaryImages()
    {
        await _imageStorageRepository.DeleteTemporaryImages();
    }
}
