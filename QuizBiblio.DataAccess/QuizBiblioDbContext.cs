using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizBiblio.Models;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess;

public class QuizBiblioDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Book> Books => Set<Book>();

    public DbSet<QuizEntity> Quizzes => Set<QuizEntity>();

    public DbSet<Models.User> Users => Set<Models.User>();

    public DbSet<Models.Theme> Themes => Set<Models.Theme>();

    public static QuizBiblioDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<QuizBiblioDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>();

        modelBuilder.Entity<QuizEntity>();

        modelBuilder.Entity<Models.Theme>();
    }

}