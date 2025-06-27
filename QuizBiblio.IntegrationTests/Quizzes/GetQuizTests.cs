using FluentAssertions;
using MongoDB.Bson;
using QuizBiblio.Models.Quiz;
using QuizBiblio.IntegrationTests.JsonParser;

namespace QuizBiblio.IntegrationTests.Quizzes;

public class GetQuizTests(QuizApiTests setup) : IClassFixture<QuizApiTests>
{
    private readonly HttpClient _guestClient = setup.Factory.GetUnauthorizedClient();

    [Fact]
    public async Task ShouldReturnQuizById()
    {
        //Arrange
        var quizId = new ObjectId("67dd6f4e9a458024c706abc8");

        // Act
        var response = await _guestClient.GetAsync($"/api/quizzes/{quizId}");

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

    [Fact]
    public async Task ShouldReturnAllQuizzes()
    {
        //Act
        var response = await _guestClient.GetAsync("/api/quizzes");

        response.EnsureSuccessStatusCode();

        var result = await ContentParser.GetRequestContent<List<QuizInfo>>(response);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);

        result.First().Id.Should().Be("67dd6f4e9a458024c706abc8");
    }
}
