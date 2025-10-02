using AutoMapper;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.DTO.Requests;
using Microsoft.AspNetCore.Authorization;

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
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin, Customer")]
        public async Task<ActionResult<BookDTO>> GetBook(Guid Id)
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
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksSorted([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var books = await bookService.GetAllBooks();
            books = books.OrderByDescending(x => x.Price).ToList();
            books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var booksDTO = mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDTO);
        }

        [HttpGet("by-title/{title}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksFiltered(string title, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var books = await bookService.GetAllBooks();
            books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!books.Any())
                return NotFound($"{title} Not Found!");
            books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var booksFilteredDTO = mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksFilteredDTO);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDTO>> AddBook([FromBody] CreatedBookRequest request)
        {
            var book = mapper.Map<Book>(request);
            var addedBook = await bookService.AddBook(book);
            await bookService.Save(book);
            var bookDTO = mapper.Map<BookDTO>(addedBook);
            return CreatedAtAction(nameof(GetBook), new { id = addedBook.Id }, bookDTO);
        }



        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDTO>> UpdateBook(Guid Id, CreatedBookRequest request)
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

            await bookService.Update(book);
            await bookService.Save(book);
            var bookDTO = mapper.Map<BookDTO>(book);
            return Ok(bookDTO);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(Guid Id)
        {
            var book = await bookService.GetBookById(Id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            bookService.Delete(book);
            await bookService.Save(book);
            return Ok();
        }
    }
}
