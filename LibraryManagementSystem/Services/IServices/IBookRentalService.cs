using LibraryManagementSystem.API.DTO;
using LibraryManagementSystem.API.DTO.Requests;
using LibraryManagementSystem.Data.Entities;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IBookRentalService
    {
        Task<IEnumerable<BookRentals>> GetAllBookRentals();
        Task<IEnumerable<BookRentals>> GetAllBookRentalsWithIncludes();
        Task<BookRentals?> GetBookRentalByIdWithIncludes(Guid rentalId);
        Task<BookRentals> GetBookRentalById(Guid id);
        Task<BookRentals> AddBookRental(BookRentals book);
        void Delete(BookRentals book);
        Task<BookRentals> Update(BookRentals bookRentals);
        Task Save(BookRentals book);
    }
}
