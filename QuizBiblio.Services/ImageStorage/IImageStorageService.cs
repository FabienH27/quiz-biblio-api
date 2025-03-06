using Microsoft.AspNetCore.Http;
using QuizBiblio.Models.Image;
using GS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Services.ImageStorage;

public interface IImageStorageService
{
    public Task<UploadResult> UploadImageAsync(IFormFile file);

    public Task<string> MoveImageToAssetsAsync(string imageUrl);
}
