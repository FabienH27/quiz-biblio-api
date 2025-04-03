using Microsoft.AspNetCore.Http;
using QuizBiblio.Models.Image;

namespace QuizBiblio.Services.ImageStorage;

public interface IImageStorageService
{
    /// <summary>
    /// Uploads an image to temporary folder
    /// </summary>
    /// <param name="file">fiile data to upload</param>
    /// <returns>image id and url</returns>
    /// <exception cref="ImageUploadExcention">exception thrown in case of invalid data</exception>
    public Task<UploadResult> UploadImageAsync(IFormFile file);

    /// <summary>
    /// Moves image to asset once the user assigned it to a quiz
    /// </summary>
    /// <param name="imageId">id of the image to move</param>
    /// <returns></returns>
    public Task MoveImageToAssetsAsync(string imageId);

    /// <summary>
    /// Gets an image from image id
    /// </summary>
    /// <param name="imageId">id of the document</param>
    /// <returns></returns>
    public Task<ImageDto> GetImageAsync(string imageId);

    /// <summary>
    /// Deletes temporary images
    /// </summary>
    /// <returns>whether images were successfully deleted</returns>
    public Task<bool> DeleteTemporaryImages();
}
