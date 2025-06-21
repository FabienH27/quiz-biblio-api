using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.Infrastructure.Storage;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services.Exceptions;
using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.UnitTests;

public class ImageUploadTests
{

    private readonly Mock<IImageStorageRepository> _imageStorageRepositoryMock;
    private readonly Mock<ICloudStorageService> _cloudStorageServiceMock;

    private readonly IOptions<BucketSettings> _bucketSettings = Options.Create<BucketSettings>(new BucketSettings() { Name = "test", QuizImageAssetsLocation = "assets/", ResizedImageWidth = 800, TemporaryImageLocation = "temp/" });

    public ImageUploadTests()
    {
        _imageStorageRepositoryMock = new Mock<IImageStorageRepository>();
        _cloudStorageServiceMock = new Mock<ICloudStorageService>();
    }

    public static TheoryData<string, string> InvalidFileData => new()
    {
        { "", "" },
        { "", "pdf" },
        { "abcd", "dfp" },
        { "abcd", "docx" },
        { new string('a', 5 * 1024 * 1024 + 1), "png" }
    };

    [Theory]
    [MemberData(nameof(InvalidFileData))]
    public async Task ShouldThrowExceptionWithEmptyFile(string content, string extension)
    {
        var service = new ImageStorageService(_imageStorageRepositoryMock.Object, _cloudStorageServiceMock.Object, _bucketSettings);

        var file = CreateFile(content, extension);

        await Assert.ThrowsAsync<ImageUploadException>(async () => await service.UploadImageAsync(file));
    }


    [Fact]
    public async Task ShouldSaveFileWhenValid()
    {
        var service = new ImageStorageService(_imageStorageRepositoryMock.Object, _cloudStorageServiceMock.Object, _bucketSettings);

        var file = CreateFile("abcd", "jpg");

        await service.UploadImageAsync(file);

        _imageStorageRepositoryMock.Verify(x => x.SaveImageAsync(It.Is<string>(x => x.StartsWith("quiz-images/"))), Times.Once);

        _imageStorageRepositoryMock.VerifyNoOtherCalls();
    }

    private FormFile CreateFile(string content, string extension)
    {
        var fileName = $"testfile.{extension}";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        _cloudStorageServiceMock.Setup(x => x.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync($"quiz-images/{fileName}");

        return new FormFile(stream, 0, stream.Length, "test_file", fileName);
    }
}
