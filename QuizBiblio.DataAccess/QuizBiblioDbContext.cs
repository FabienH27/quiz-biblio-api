using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess;

public class QuizBiblioDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Models.Quiz> Quizzes => Set<Models.Quiz>();

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

        modelBuilder.Entity<Models.Quiz>();

        modelBuilder.Entity<Models.Theme>();
    }

}