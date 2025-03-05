using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services.Exceptions;
using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.Controllers;

[Route("api/[controller]")]
[ApiController]
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

            return Ok(new { message = "File uploaded successfully.", url = uploadResult.ImageUrl });
        
        }catch(GoogleApiException ex)
        {
            return BadRequest($"Could not upload image to bucket: {ex.Message}");
        }catch(ImageUploadExcention uploadEx)
        {
            return BadRequest($"An error occured while uploading image : {uploadEx.Message}");
        }
    }
}
