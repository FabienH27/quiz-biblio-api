using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.JobScheduler.Jobs;

public class StorageCleanupJob(IImageStorageService imageStorageService)
{
    private readonly IImageStorageService _imageStorageService = imageStorageService;

    public async Task PerformCleanup()
    {
        await _imageStorageService.DeleteTemporaryImages();
    }
}
