using MongoDB.Driver;
using QuizBiblio.DataAccess;
using QuizBiblio.Models;

namespace QuizBiblio.Services;
public class BooksService
{

    private readonly BooksRepository _booksRepository;

    public BooksService(BooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }

    public async Task<List<Book>> GetAsync() => await _booksRepository.GetBooksAsync();

    //public async Task<Book?> GetAsync(string id) => await _booksRepository.GetAsync(id);

    //public async Task CreateAsync(Book newBook) => await _booksRepository.CreateAsync(newBook);

    //public async Task UpdateAsync(string id, Book updatedBook) => await _booksRepository.UpdateAsync(id, updatedBook);

    //public async Task RemoveAsync(string id) => await _booksRepository.RemoveAsync(id);

}
