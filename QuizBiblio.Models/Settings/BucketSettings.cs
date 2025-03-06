namespace QuizBiblio.Models.Settings;

public class BucketSettings
{
    public required string Name { get; set; }

    public required string TemporaryImageLocation { get; init; }

    public required string QuizImageAssetsLocation { get; init; }
}
