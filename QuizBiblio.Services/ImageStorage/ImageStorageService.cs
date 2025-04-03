using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.Models.Image;
using QuizBiblio.Services.Exceptions;

namespace QuizBiblio.Services.ImageStorage;

public class ImageStorageService : IImageStorageService
{

    private readonly IImageStorageRepository _imageStorageRepository;

    public ImageStorageService(IImageStorageRepository imageStorageRepository)
    {
        _imageStorageRepository = imageStorageRepository;
    }

    /// <summary>
    /// Uploads an image to temporary folder
    /// </summary>
    /// <param name="file">fiile data to upload</param>
    /// <returns>image id and url</returns>
    /// <exception cref="ImageUploadExcention">exception thrown in case of invalid data</exception>
    public async Task<UploadResult> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ImageUploadExcention("No file uploaded.");
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(file.FileName, out var contentType))
        {
            throw new ImageUploadExcention("Could not determine file type.");
        }

        var allowedExtensions = new HashSet<string> { ".png", ".jpg", ".jpeg", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension) || !contentType.StartsWith("image/"))
        {
            throw new ImageUploadExcention("Invalid file type. Only images are allowed.");
        }

        if (file.Length > 5 * 1024 * 1024)
        {
            throw new ImageUploadExcention("File exceeds the 5MB limit.");
        }

        return await _imageStorageRepository.UploadImageAsync(file, contentType);
    }

    /// <summary>
    /// Moves image to asset once the user assigned it to a quiz
    /// </summary>
    /// <param name="imageId">id of the image to move</param>
    /// <returns></returns>
    public async Task MoveImageToAssetsAsync(string imageId)
    {
        await _imageStorageRepository.MoveImageToAssets(imageId);
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
    /// <returns>whether images were successfully deleted</returns>
    public async Task<bool> DeleteTemporaryImages()
    {
        return await _imageStorageRepository.DeleteTemporaryImages();
    }
}
