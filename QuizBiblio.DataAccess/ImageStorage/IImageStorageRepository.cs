using Microsoft.AspNetCore.Http;
using QuizBiblio.Models.Image;
using GS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.DataAccess.ImageStorage;

public interface IImageStorageRepository
{
    public Task<UploadResult> UploadImageAsync(IFormFile file, string contentType);
}
