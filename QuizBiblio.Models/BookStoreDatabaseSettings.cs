namespace QuizBiblio.Models;

public class BookStoreDatabaseSettings
{
    public required string ConnectionString { get; set; }

    public required string DatabaseName { get; set; }

    public required string BooksCollectionName { get; set; }
}