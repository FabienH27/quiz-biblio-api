using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models;
using QuizBiblio.Models.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizBiblio.IntegrationTests.Quizzes;

public class GetQuizByIdTests : IClassFixture<QuizApiTests>
{
    private readonly IMongoDbContext _dbContext;
    private readonly HttpClient _client;

    public GetQuizByIdTests(QuizApiTests setup)
    {
        _client = setup.Factory.CreateClient();

        var scopeFactory = setup.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<IMongoDbContext>();
    }

    [Fact]
    public async Task ShouldReturnQuizById()
    {
        var quiz = new QuizEntity
        {
            Title = "Test Quiz",
            Creator = new QuizCreator { Id = ObjectId.GenerateNewId().ToString(), Name = "Test_User" },
            Id = ObjectId.GenerateNewId().ToString(),
            Questions = [
                new Question(){
                    Text = "Test question",
                    Proposals = [
                        new Proposal{
                            Text = "a"
                        },
                        new Proposal{
                            Text= "b"
                        }
                    ],
                    CorrectProposalIds = [0],
                    Details = "because"
                }
            ]
        };

        await _dbContext.GetCollection<QuizEntity>("Quizzes").InsertOneAsync(quiz);

        // Act
        var response = await _client.GetAsync($"/api/quizzes/{quiz.Id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<QuizDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        result.Should().NotBeNull();
        result.Title.Should().Be("Test Quiz");
        result.Questions.Should().HaveCount(1);
        result.Questions[0].Text.Should().Be("Test question");
        result.Creator.Should().NotBeNull();
    }
}
