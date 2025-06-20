using Google.Api.Gax;
using QuizBiblio.Models.Image;
using GAS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Services.CloudStorage;

public interface ICloudStorageService
{
    public Task<string> SaveFileAsync(Stream stream, string fileName, string contentType);

    public Task<string> MoveImageToAssetsAsync(ImageEntity image);

    public Task<Page<GAS.Object>> ListImages();

    public Task<bool> DeleteTemporaryImages();
}
