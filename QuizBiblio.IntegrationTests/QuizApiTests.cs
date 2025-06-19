using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Quiz;
using System.Text.Json;
using Testcontainers.MongoDb;
using QuizBiblio.IntegrationTests.JsonParser;
using MongoDB.Driver;

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

    public required IMongoDbContext _dbContext;

    public IMongoCollection<QuizEntity> Quizzes => _dbContext.GetCollection<QuizEntity>("Quizzes");

    public async Task InitializeAsync()
    {
        await _mongoContainer.StartAsync();

        var host = _mongoContainer.Hostname;
        var port = _mongoContainer.GetMappedPublicPort(27017);

        var connectionString = $"mongodb://test:test@{host}:{port}/quiz-tests?authSource=admin";

        Factory = new QuizBiblioApplicationFactory(connectionString, "quiz-tests");

        var scopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();

        await PopulateDatabase();
    }

    public Task DisposeAsync() => _mongoContainer.DisposeAsync().AsTask();

    public async Task CreateQuiz(QuizEntity quizEntity) => await Quizzes.InsertOneAsync(quizEntity);

    public async Task PopulateDatabase()
    {
        var json = await File.ReadAllTextAsync("TestData/quizzes.json");
        var quizzes = JsonSerializer.Deserialize<List<QuizEntity>>(json, ContentParser.SerializerOptions);

        await Quizzes.InsertManyAsync(quizzes);
    }
}