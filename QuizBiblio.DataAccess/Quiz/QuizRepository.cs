using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository : IQuizRepository
{

    private readonly IMongoDbContext _dbContext;

    IMongoCollection<QuizEntity> Quizzes => _dbContext.GetCollection<QuizEntity>("Quizzes");

    public QuizRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateQuiz(QuizEntity quiz)
    {
        throw new NotImplementedException();
    }

    public async Task<List<QuizEntity>> GetQuizzesAsync()
    {
        return await Quizzes.AsQueryable().ToListAsync();
    }

    public Task<List<QuizEntity>> GetUserQuizzesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateQuiz(QuizEntity quiz)
    {
        throw new NotImplementedException();
    }

    //private readonly QuizBiblioDbContext _dbContext;

    //DbSet<QuizEntity> Quizzes => _dbContext.Quizzes;

    //public QuizRepository(QuizBiblioDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

    //public async Task<List<QuizEntity>> GetQuizzesAsync() => await Quizzes.ToListAsync();

    //public async Task<List<QuizEntity>> GetUserQuizzesAsync(string userId)
    //{
    //    var projection = Builders<QuizEntity>.Projection.Expression(quiz => new QuizInfo
    //    {
    //        Id = userId,
    //        Title = quiz.Title,
    //        ImageId = quiz.ImageId,
    //        Themes = quiz.Themes,
    //    });

    //    return await Quizzes.Where(quiz => quiz.Creator.CreatorId == ObjectId.Parse(userId)).ToListAsync();
    //}

    ////Non async : standard use case, refer to Add() method documentation
    //public void CreateQuiz(QuizEntity quiz)
    //{
    //    Quizzes.Add(quiz);
    //    _dbContext.SaveChanges();
    //}

    //public void UpdateQuiz(QuizEntity quiz)
    //{
    //    Quizzes.Update(quiz);
    //    _dbContext.SaveChanges();
    //}
}
