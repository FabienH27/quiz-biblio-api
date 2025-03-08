
namespace QuizBiblio.Models.Image;

public class ImageUpload
{
    /// <summary>
    /// File name given by user
    /// </summary>
    public string UploadName { get; set; } = string.Empty;

    /// <summary>
    /// Content-type of the file
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Content of the file
    /// </summary>
    public byte[] File { get; set; } = [];
}
