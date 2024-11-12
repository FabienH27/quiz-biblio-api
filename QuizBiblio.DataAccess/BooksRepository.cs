using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizBiblio.Models;

namespace QuizBiblio.DataAccess;

public class BooksRepository
{
    //private readonly IMongoCollection<Book> _booksCollection;

    private readonly QuizBiblioDbContext _dbContext;

    DbSet<Book> Books => _dbContext.Books;

    public BooksRepository(QuizBiblioDbContext dbContext)
    {

        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetBooksAsync() => await Books.ToListAsync();
}
