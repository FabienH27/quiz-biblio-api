using Microsoft.EntityFrameworkCore;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository(QuizBiblioDbContext dbContext) : IQuizRepository
{
    DbSet<Models.Quiz> Quizzes => dbContext.Quizzes;

    public async Task<List<Models.Quiz>> GetQuizzesAsync() => await Quizzes.ToListAsync();

    //Non async : standard use case, refer to Add() method documentation
    public void CreateQuiz(Models.Quiz quiz)
    {
        Quizzes.Add(quiz);
        dbContext.SaveChanges();
    }

}
