using Microsoft.AspNetCore.Http;
using QuizBiblio.Models.Image;
using GS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Services.ImageStorage;

public interface IImageStorageService
{
    public Task<UploadResult> UploadImageAsync(IFormFile file);

    public Task MoveImageToAssetsAsync(string imageId);

    public Task<ImageDto> GetImageAsync(string imageId);
}
