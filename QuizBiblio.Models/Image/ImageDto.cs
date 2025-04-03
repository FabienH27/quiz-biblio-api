namespace QuizBiblio.Models.Image;

public class ImageDto
{
    public required string OriginalUrl { get; set; }

    public string? ResizedUrl { get; set; }

    public bool? IsPermanent { get; set; }
}
