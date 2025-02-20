﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Helper;
using QuizBiblio.Models.UserQuiz;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.UserQuizScore;
using System.Security.Claims;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserQuizScoreController(IUserQuizScoreService userScoreService) : ControllerBase
{
    [HttpGet("{id:length(24)}")]
    public async Task<UserQuizScoreEntity?> GetUser(string id)
    {
        return await userScoreService.GetUserScoreAsync(id);
    }

    [HttpPost]
    [Authorize]
    public async Task SaveUserScore([FromBody] UserScoreRequest userScore)
    {
        var userQuizScore = new UserQuizScoreEntity
        {
            UserId = User.Claims.GetUserId(),
            UserName = User.Claims.GetUserName(),
            Score = userScore.UserScore
        };

        await userScoreService.SaveUserScoreAsync(userQuizScore);
    }
}
