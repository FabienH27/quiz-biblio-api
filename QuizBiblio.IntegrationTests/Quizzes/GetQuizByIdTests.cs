using FluentAssertions;
using MongoDB.Bson;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Quiz;
using QuizBiblio.IntegrationTests.JsonParser;

namespace QuizBiblio.IntegrationTests.Quizzes;

public class GetQuizByIdTests(QuizApiTests setup) : IClassFixture<QuizApiTests>
{
    private readonly IMongoDbContext _dbContext = setup._dbContext;
    private readonly HttpClient _client = setup.Factory.CreateClient();

    //TODO: Logout of gcloud locally and try running test again

    [Fact]
    public async Task ShouldReturnQuizById()
    {
        var quizId = new ObjectId("67dd6f4e9a458024c706abc8");

        // Act
        var response = await _client.GetAsync($"/api/quizzes/{quizId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await ContentParser.GetRequestContent<QuizDto>(response);

        result.Should().NotBeNull();
        result.Title.Should().Be("Quiz Test");
        result.Id.Should().Be(quizId.ToString());
        result.Questions.Should().HaveCount(1);
        result.Questions.ElementAt(0).Text.Should().Be("Question A");
        result.Creator.Should().NotBeNull();
    }
}
