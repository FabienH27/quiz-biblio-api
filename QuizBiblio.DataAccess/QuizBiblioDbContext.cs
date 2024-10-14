using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using QuizBiblio.Models;


namespace QuizBiblio.DataAccess;

public class QuizBiblioDbContext : DbContext
{

    public DbSet<Book> Books { get; init; }

    public static QuizBiblioDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<QuizBiblioDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    public QuizBiblioDbContext(DbContextOptions options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>();
    }

}