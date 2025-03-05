
namespace QuizBiblio.Models.Image;

public class ImageUpload
{
    /// <summary>
    /// File name given by user
    /// </summary>
    public string UploadName { get; set; }
    
    /// <summary>
    /// Content-type of the file
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// Content of the file
    /// </summary>
    public byte[] File { get; set; }
}
