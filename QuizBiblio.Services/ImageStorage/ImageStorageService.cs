using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.Models.Image;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services.Exceptions;
using GS = Google.Apis.Storage.v1.Data;

namespace QuizBiblio.Services.ImageStorage;

public class ImageStorageService : IImageStorageService
{

    private readonly IImageStorageRepository _imageStorageRepository;

    public ImageStorageService(IImageStorageRepository imageStorageRepository)
    {
        _imageStorageRepository = imageStorageRepository;
    }

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
}
