using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizBiblio.Models;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.UserQuiz;

namespace QuizBiblio.DataAccess;

public class QuizBiblioDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<QuizEntity> Quizzes => Set<QuizEntity>();

    public DbSet<Models.User> Users => Set<Models.User>();

    public DbSet<Models.Theme> Themes => Set<Models.Theme>();

    public DbSet<UserQuizScoreEntity> UserQuizScores => Set<UserQuizScoreEntity>();

    public static QuizBiblioDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<QuizBiblioDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuizEntity>();

        modelBuilder.Entity<Models.Theme>();
    }

}