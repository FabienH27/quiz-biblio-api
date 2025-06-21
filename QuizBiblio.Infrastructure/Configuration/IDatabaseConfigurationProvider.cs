namespace QuizBiblio.Infrastructure.Configuration;

public interface IDatabaseConfigurationProvider
{
    string? GetConnectionString();
    string? GetDatabaseName();
}
