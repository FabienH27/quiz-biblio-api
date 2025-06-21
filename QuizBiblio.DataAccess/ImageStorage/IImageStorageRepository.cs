using QuizBiblio.Models.Image;

namespace QuizBiblio.DataAccess.ImageStorage;

public interface IImageStorageRepository
{
    public Task<UploadResult> SaveImageAsync(string storageLocation);
    public Task MoveImageToAssetsAsync(ImageEntity image);

    public Task<ImageEntity> GetImageAsync(string imageId);

    public Task DeleteTemporaryImages();
}
