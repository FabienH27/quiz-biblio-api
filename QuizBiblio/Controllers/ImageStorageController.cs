using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models.Image;
using QuizBiblio.Services.Exceptions;
using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageStorageController : ControllerBase
{
    private readonly ILogger<ImageStorageController> _logger;

    private readonly IImageStorageService _imageStorageService;

    public ImageStorageController(
        ILogger<ImageStorageController> logger,
        IImageStorageService imageStorageService)
    {
        _logger = logger;
        _imageStorageService = imageStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            var uploadResult = await _imageStorageService.UploadImageAsync(file);

            return Ok(new { message = "File uploaded successfully.", id = uploadResult.ImageId, url = uploadResult.ImageUrl });
        
        }catch(GoogleApiException ex)
        {
            return BadRequest($"Could not upload image to bucket: {ex.Message}");
        }catch(ImageUploadExcention uploadEx)
        {
            return BadRequest($"An error occured while uploading image : {uploadEx.Message}");
        }
    }

    /// <summary>
    /// Gets an image from a given id
    /// </summary>
    /// <param name="imageId">id of the image to fetch</param>
    /// <returns>url of the image</returns>
    [HttpGet("{imageId}")]
    public async Task<ImageDto> GetImageAsync(string imageId)
    {
        return await _imageStorageService.GetImageAsync(imageId);
    }
}
