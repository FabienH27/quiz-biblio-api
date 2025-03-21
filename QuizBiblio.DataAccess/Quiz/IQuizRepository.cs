﻿using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public interface IQuizRepository
{
    public Task<List<QuizInfo>> GetQuizzesAsync();

    public Task<QuizDto> GetQuiz(string quizId);

    public Task<List<QuizInfo>> GetUserQuizzesAsync(string userId);

    public Task<bool> CreateQuiz(QuizEntity quiz);

    public Task<bool> UpdateQuiz(QuizEntity quiz);

    public Task DeleteQuizAsync(string quizId);
}
