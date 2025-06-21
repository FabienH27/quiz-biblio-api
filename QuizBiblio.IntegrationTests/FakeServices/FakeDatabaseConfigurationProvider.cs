using QuizBiblio.Infrastructure.Configuration;

namespace QuizBiblio.IntegrationTests.FakeServices;

public class FakeDatabaseConfigurationProvider(string connectionString, string databaseName) : IDatabaseConfigurationProvider
{
    private readonly string _connectionString = connectionString;
    private readonly string _databaseName = databaseName;

    public string GetConnectionString() => _connectionString;

    public string GetDatabaseName() => _databaseName;
}
