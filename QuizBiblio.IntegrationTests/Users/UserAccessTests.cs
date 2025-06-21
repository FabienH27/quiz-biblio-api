using FluentAssertions;
using System.Net;

namespace QuizBiblio.IntegrationTests.Users;

public class UserAccessTests(QuizApiTests setup) : IClassFixture<QuizApiTests>
{
    [Fact]
    public async Task GetUsers_ShouldReturn401_WhenNotLoggedIn()
    {
        var client = setup.Factory.GetUnauthorizedClient();

        var response = await client.GetAsync("api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUsers_ShouldReturn403_WhenLoggedInAsMember()
    {
        var client = setup.Factory.GetAuthorizedClient();

        var response = await client.GetAsync("api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUsers_ShouldReturn200_WhenLoggedInAsAdmin()
    {
        var client = setup.Factory.GetAdminClient();

        var response = await client.GetAsync("api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
