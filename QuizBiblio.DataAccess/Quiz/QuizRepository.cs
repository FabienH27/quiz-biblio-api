using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository : IQuizRepository
{
    private readonly QuizBiblioDbContext _dbContext;

    DbSet<QuizEntity> Quizzes => _dbContext.Quizzes;

    public QuizRepository(QuizBiblioDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<QuizEntity>> GetQuizzesAsync() => await Quizzes.ToListAsync();

    public async Task<List<QuizEntity>> GetUserQuizzesAsync(string userId)
    {
        return await Quizzes.Where(quiz => quiz.Creator.CreatorId == ObjectId.Parse(userId)).ToListAsync();
    }

    //Non async : standard use case, refer to Add() method documentation
    public void CreateQuiz(QuizEntity quiz)
    {
        Quizzes.Add(quiz);
        _dbContext.SaveChanges();
    }

    public void UpdateQuiz(QuizEntity quiz)
    {
        Quizzes.Update(quiz);
        _dbContext.SaveChanges();
    }
}
