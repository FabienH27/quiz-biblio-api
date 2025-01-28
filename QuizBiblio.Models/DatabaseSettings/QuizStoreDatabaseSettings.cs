namespace QuizBiblio.Models.DatabaseSettings;

public class QuizStoreDatabaseSettings
{
    public required string ConnectionString { get; set; }

    public required string DatabaseName { get; set; }

    public required string QuizzesCollectionName { get; set; }
}