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

    //public async Task<List<Book>> GetAsync() => await _booksCollection.Find(_ => true).ToListAsync();

    //public async Task<Book?> GetAsync(string id) =>
    //    await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    //public async Task CreateAsync(Book newBook) =>
    //    await _booksCollection.InsertOneAsync(newBook);

    //public async Task UpdateAsync(string id, Book updatedBook) =>
    //    await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    //public async Task RemoveAsync(string id) =>
    //    await _booksCollection.DeleteOneAsync(x => x.Id == id);
}
