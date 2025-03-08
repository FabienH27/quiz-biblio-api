using MongoDB.Bson.Serialization.Attributes;

namespace QuizBiblio.Models.Image;

public class UploadResult
{
    public required string ImageId { get; set; }

    public required string ImageUrl { get; set; }
}
