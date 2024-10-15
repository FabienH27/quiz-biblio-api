using Microsoft.EntityFrameworkCore;

namespace QuizBiblio.DataAccess.Quiz;

public class QuizRepository(QuizBiblioDbContext dbContext) : IQuizRepository
{
    DbSet<Models.Quiz.Quiz> Quizzes => dbContext.Quizzes;

    public async Task<List<Models.Quiz.Quiz>> GetQuizzesAsync() => await Quizzes.ToListAsync();

}
