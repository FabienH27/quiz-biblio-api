using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.JobScheduler.Jobs;

public class StorageCleanupJob
{
    private IImageStorageService _imageStorageService;

    public StorageCleanupJob(IImageStorageService imageStorageService)
    {
        _imageStorageService = imageStorageService;
    }

    public async Task<bool> PerformCleanup()
    {
        return await _imageStorageService.DeleteTemporaryImages();
    }
}
