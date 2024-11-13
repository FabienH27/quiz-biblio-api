using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess;

public class QuizBiblioDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Book> Books { get; init; }

    public DbSet<Models.Quiz> Quizzes {  get; init; }  

    public DbSet<Models.User> Users { get; init; }

    public static QuizBiblioDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<QuizBiblioDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>();

        modelBuilder.Entity<Models.Quiz>();
    }

}