﻿using Microsoft.AspNetCore.Mvc;
using QuizBiblio.Models;
using QuizBiblio.Services;

namespace QuizBiblio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(BooksService booksService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Book>> Get() => await booksService.GetAsync();

    //[HttpPost]
    //public async Task<IActionResult> Post(Book newBook)
    //{
    //    await _booksService.CreateAsync(newBook);

    //    return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    //}

    //[HttpPut("{id:length(24)}")]
    //public async Task<IActionResult> Update(string id, Book updatedBook)
    //{
    //    var book = await _booksService.GetAsync(id);

    //    if (book is null)
    //    {
    //        return NotFound();
    //    }

    //    updatedBook.Id = book.Id;

    //    await _booksService.UpdateAsync(id, updatedBook);

    //    return NoContent();
    //}

    //[HttpDelete("{id:length(24)}")]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    var book = await _booksService.GetAsync(id);

    //    if (book is null)
    //    {
    //        return NotFound();
    //    }

    //    await _booksService.RemoveAsync(id);

    //    return NoContent();
    //}
}