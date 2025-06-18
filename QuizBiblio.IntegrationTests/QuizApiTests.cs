using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using QuizBiblio.DataAccess.QbDbContext;
using Testcontainers.MongoDb;

namespace QuizBiblio.IntegrationTests;

public class QuizApiTests : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(27017, true) // expose automatiquement un port local
        .WithEnvironment("MONGO_INITDB_DATABASE", "quiz-tests")
        .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "test")
        .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "test")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
        .Build();

    internal WebApplicationFactory<Program> Factory { get; private set; } = null!;
    public IServiceProvider Services => Factory.Services;

    public async Task InitializeAsync()
    {
        await _mongoContainer.StartAsync();

        var host = _mongoContainer.Hostname;
        var port = _mongoContainer.GetMappedPublicPort(27017);

        var connectionString = $"mongodb://test:test@{host}:{port}/quiz-tests?authSource=admin";

        Factory = new QuizBiblioApplicationFactory(connectionString, "quiz-tests");
    }

    public Task DisposeAsync()
    {
        return _mongoContainer.DisposeAsync().AsTask();
    }
}
