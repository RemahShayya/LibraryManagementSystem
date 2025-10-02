using AutoMapper;
using LibraryManagementSystem.API.DTO;
using LibraryManagementSystem.API.DTO.Requests;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookRentalController : ControllerBase
    {
        private readonly IBookRentalService _rentalService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IReturnedRentalService _returnedRentalService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BookRentalController(IBookRentalService rentalService, IBookService bookService, IUserService userService, IMapper mapper, IConfiguration configuration, IReturnedRentalService returnedRentalService)
        {
            _rentalService = rentalService;
            _bookService = bookService;
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
            _returnedRentalService = returnedRentalService;
        }

        [HttpPost("rent")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> RentBook(CreateBookRentalRequest request)
        {
            var book = await _bookService.GetBookById(request.BookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }
            var customer = await _userService.GetUserById(request.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }
            var pricePerDay = _configuration.GetValue<double>("RentalSettings:PricePerDay");
            var days = (request.RentEndDate - DateTime.UtcNow).Days;
            var quarterBookPrice = (book.Price ?? 0) / 4;
            var rent = new BookRentals
            {
                BookId = request.BookId,
                CustomerId = request.CustomerId,
                RentStartDate = DateTime.UtcNow,
                RentEndDate = request.RentEndDate,
                Quantity = request.Quantity,
                Price = pricePerDay * days * request.Quantity * (double)quarterBookPrice,
            };
            if (book.Quantity < rent.Quantity)
            {
                return BadRequest("Not enough copies available.");
            }
            await _rentalService.AddBookRental(rent);
            await _rentalService.Save(rent);

            book.Quantity -= rent.Quantity;
            await _bookService.Save(book);

            return Ok("Rental Added Successfully");
        }

        [HttpPost("return/{rentalId}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> ReturnBook(Guid rentalId)
        {
            var rental = await _rentalService.GetBookRentalByIdWithIncludes(rentalId);
            if (rental == null)
            {
                return NotFound("Rental not found.");
            }

            rental.RentEndDate = DateTime.UtcNow;

            var days = (rental.RentEndDate - rental.RentStartDate).Days;
            var pricePerDay = _configuration.GetValue<double>("RentalSettings:PricePerDay");
            var quarterBookPrice = (rental.Book.Price ?? 0) / 4;
            rental.Price = days * pricePerDay * rental.Quantity * (double)quarterBookPrice;

            var returnedRental = new ReturnedRental
            {
                Id = Guid.NewGuid(),
                BookId = rental.BookId,
                CustomerId = rental.CustomerId,
                RentedAt = rental.RentStartDate,
                ReturnedAt = rental.RentEndDate,
                BookTitle = rental.Book.Title,
                CustomerName = rental.Customer.Email,
                Quantity = rental.Quantity,
                Price = rental.Price,
            };

            await _returnedRentalService.AddReturnedRental(returnedRental);
            await _returnedRentalService.Save(returnedRental);

            var book = await _bookService.GetBookById(rental.Book.Id);
            if (book != null)
            {
                book.Quantity += rental.Quantity;
                await _bookService.Save(book);
            }
            _rentalService.Delete(rental);
            await _rentalService.Save(rental);


            var rentalDto = _mapper.Map<BookRentalDTO>(rental);
            return Ok(rentalDto);
        }

        [HttpDelete("{rentalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRental(Guid rentalId)
        {
            var bookRental = await _rentalService.GetBookRentalByIdWithIncludes(rentalId);
            if (bookRental == null)
                return NotFound("Rental not found.");

            var book = await _bookService.GetBookById(bookRental.BookId);
            if (book != null)
            {
                book.Quantity += bookRental.Quantity;
                await _bookService.Save(book);
            }
            _rentalService.Delete(bookRental);
            await _rentalService.Save(bookRental);

            return Ok("Rental deleted successfully.");
        }

        [HttpPut("{rentalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRental(Guid rentalId, [FromBody] CreateBookRentalRequest request)
        {
            var rental = await _rentalService.GetBookRentalById(rentalId);
            var book = await _bookService.GetBookById(request.BookId);
            var customer = await _userService.GetUserById(request.CustomerId);

            if (customer == null || book == null || rental == null)
                return NotFound("Wrong Rental, Book, or Customer ID.");

            int quantityDifference = request.Quantity - rental.Quantity;

            book.Quantity -= quantityDifference;
            await _bookService.Save(book);
            
            var pricePerDay = _configuration.GetValue<double>("RentalSettings:PricePerDay");
            var quarterBookPrice = (book.Price ?? 0) / 4;
            rental.Price = (request.Quantity * (rental.RentEndDate - rental.RentStartDate).Days) * pricePerDay * (double)quarterBookPrice;
            rental.CustomerId = request.CustomerId;
            rental.BookId = request.BookId;
            rental.RentEndDate = request.RentEndDate;
            rental.Quantity = request.Quantity;
            await _rentalService.Save(rental);

            return Ok("Rental updated successfully.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<BookRentalDTO>> GetAllRentals()
        {
            if (User.IsInRole("Admin"))
            {
                var rentals = await _rentalService.GetAllBookRentalsWithIncludes();
                var rentalDto = _mapper.Map<IEnumerable<BookRentalDTO>>(rentals);
                return Ok(rentalDto);
            }
            else if (User.IsInRole("Customer"))
            {
                var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var rentals = await _rentalService.GetAllBookRentalsWithIncludes();
                var customerRentals = rentals.Where(r => r.CustomerId.ToString() == customerId).ToList();
                var customerRentalsDTO = _mapper.Map<IEnumerable<BookRentalDTO>>(customerRentals);
                return Ok(customerRentalsDTO);
            }
            return Ok();
        }

        [HttpGet("{rentalId}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<BookRentalDTO>>> GetRentalById(Guid rentalId)
        {
            var rental = await _rentalService.GetBookRentalByIdWithIncludes(rentalId);
            if (rental == null) return NotFound("Rental Not Found");
            var rentalDto=_mapper.Map<BookRentalDTO>(rental);
            return Ok(rentalDto);
        }

        [HttpGet("customer/{email}/summary")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<IEnumerable<BookRentalDTO>>> GetCustomerRentalSummary(string customerEmail)
        {
            var allUsers = await _userService.GetAllUsers();
            var customer = allUsers.FirstOrDefault(u => u.Email == customerEmail);
            if (customer == null)
                throw new Exception("Customer not found");

            var rentals = await _rentalService.GetAllBookRentalsWithIncludes();


            var customerRentals = rentals.Where(r => r.CustomerId.ToString() == customer.Id).ToList();

            var customerRentalsDTO = _mapper.Map<IEnumerable<BookRentalDTO>>(customerRentals);
            return Ok(customerRentalsDTO);

        }

    }
}
