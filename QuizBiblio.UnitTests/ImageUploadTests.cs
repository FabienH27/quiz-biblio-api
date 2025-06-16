using Microsoft.AspNetCore.Http;
using Moq;
using QuizBiblio.DataAccess.ImageStorage;
using QuizBiblio.Services.Exceptions;
using QuizBiblio.Services.ImageStorage;

namespace QuizBiblio.UnitTests;

public class ImageUploadTests
{

    private readonly Mock<IImageStorageRepository> _imageStorageRepositoryMock;

    public ImageUploadTests()
    {
        _imageStorageRepositoryMock = new Mock<IImageStorageRepository>();
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
        var service = new ImageStorageService(_imageStorageRepositoryMock.Object);

        var file = CreateFile(content, extension);

        await Assert.ThrowsAsync<ImageUploadException>(async () => await service.UploadImageAsync(file));
    }


    [Fact]
    public async Task ShouldSaveFileWhenValid()
    {
        var service = new ImageStorageService(_imageStorageRepositoryMock.Object);
        
        var file = CreateFile("abcd", "jpg");

        await service.UploadImageAsync(file);

        _imageStorageRepositoryMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.Is<string>(x => x.StartsWith("image/"))), Times.Once);

        _imageStorageRepositoryMock.VerifyNoOtherCalls();

    }

    private static FormFile CreateFile(string content, string extension)
    {
        var fileName = $"testfile.{extension}";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        return new FormFile(stream, 0, stream.Length, "test_file", fileName);
    }
}
