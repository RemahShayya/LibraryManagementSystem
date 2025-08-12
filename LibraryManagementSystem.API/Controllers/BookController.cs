using AutoMapper;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LibraryManagementSystem.Data.DTO;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IMapper mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            this.bookService = bookService;
            this.mapper=mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid Id)
        {
            var book = await bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound("Book not found!");
            }
            var bookDTO = mapper.Map<BookDTO>(book);
            return Ok(bookDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            List<Book> books = await bookService.GetAllBooks();
            var bookDTO=mapper.Map<BookDTO>(books);
            return Ok(bookDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(CreatedBookRequest request)
        {
            var book=mapper.Map<Book>(request);
            var addedBook = await bookService.AddBook(book);
            await bookService.Save(book);
            var bookDTO = mapper.Map<BookDTO>(addedBook);
            return CreatedAtAction(nameof(bookService.GetBookById), bookDTO);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateBook(Guid Id, CreatedBookRequest request)
        {
            var bookUpdated = mapper.Map<Book>(request);
            var book = await bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound("Book Not Found");
            }
            book.Author = bookUpdated.Author;
            book.Title = bookUpdated.Title;
            book.Description = bookUpdated.Description;
            book.Price = bookUpdated.Price;
            book.LastModifiedAt = DateTime.UtcNow;
            book.LastModifiedBy = bookUpdated.LastModifiedBy;

            await bookService.Update(book, bookUpdated);
            await bookService.Save(book);
            var bookDTO = mapper.Map<BookDTO>(book);
            return Ok(bookDTO);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBook(Guid Id)
        {
            var book= await bookService.GetBookById(Id);
            if(book == null)
            {
                return NotFound("Book not found");
            }
            await bookService.Delete(Id);
            await bookService.Save(book);
            return Ok();
        }
    }
}
