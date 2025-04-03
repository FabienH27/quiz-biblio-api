using Microsoft.AspNetCore.Http;
using QuizBiblio.Models.Image;

namespace QuizBiblio.DataAccess.ImageStorage;

public interface IImageStorageRepository
{
    public Task<UploadResult> UploadImageAsync(IFormFile file, string contentType);
    public Task MoveImageToAssets(string imageId);

    public Task<ImageEntity> GetImageAsync(string imageId);

    public Task<bool> DeleteTemporaryImages();
}
