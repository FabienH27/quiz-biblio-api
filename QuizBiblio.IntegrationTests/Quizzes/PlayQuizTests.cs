using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using QuizBiblio.IntegrationTests.JsonParser;
using QuizBiblio.Models.UserQuizScore;
using System.Net;
using System.Net.Http.Json;

namespace QuizBiblio.IntegrationTests.Quizzes;

public class PlayQuizTests(QuizApiTests setup) : IClassFixture<QuizApiTests>
{
    private readonly HttpClient _guestClient = setup.Factory.GetUnauthorizedClient();
    private readonly HttpClient _userClient = setup.Factory.GetUnauthorizedClient();

    [Fact]
    public async Task Should_Compute_Score_And_Save_Score_For_Guest()
    {
        //Arrange
        var guestAnswers = new QuizAnswersRequest()
        {
            QuizId = "67dd6f4e9a458024c706abc8",
            Answers =
            [
                new AnswerRequest()
                {
                    IsCorrect = true,
                    QuestionId = 0,
                    Answers = [0]
                }
            ]
        };

        using JsonContent answersContent = JsonContent.Create(guestAnswers);

        var cookieContainer = new CookieContainer();

        var handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,
            AllowAutoRedirect = false
        };

        var cookieHandler = new CookieContainerHandler();

        var initResponse = await _guestClient.PostAsync("/api/guest/init-session?userName=test-user", null);

        var uri = new Uri(_guestClient.BaseAddress?.ToString() ?? "http://localhost");
        var cookies = cookieHandler.Container.GetCookies(uri);

        //Act
        var response = await _guestClient.PostAsync("/api/quizplay/submit-answers", answersContent);

        response.EnsureSuccessStatusCode();

        var result = await ContentParser.GetRequestContent<bool>(response);

        result.Should().Be(true);
    }
}
