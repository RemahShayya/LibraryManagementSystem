using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class BookRentalServices : IBookRentalService
    {
        private readonly ILibraryGenericRepo<BookRentals> repo;

        public BookRentalServices(ILibraryGenericRepo<BookRentals> repo)
        {
            this.repo = repo;
        }

        public async Task<BookRentals> AddBookRental(BookRentals bookRentals)
        {
            return await repo.Add(bookRentals);
        }

        public void Delete(BookRentals book)
        {
            repo.Delete(book);
        }

        public async Task<IEnumerable<BookRentals>> GetAllBookRentals()
        {
            return await repo.GetAll();
        }

        public async Task<BookRentals> GetBookRentalById(Guid id)
        {
            return await repo.Get(id);
        }

        public async Task<BookRentals> Update(BookRentals bookRentals)
        {
            return repo.Update(bookRentals);
        }
        public async Task Save(BookRentals book)
        {
            await repo.SaveAsync();
        }
        public async Task<IEnumerable<BookRentals>> GetAllBookRentalsWithIncludes()
        {
            return await repo.GetAllWithIncludesAsync(
                r => r.Customer,
                r => r.Book
            );
        }

        public async Task<BookRentals?> GetBookRentalByIdWithIncludes(Guid rentalId)
        {
            var rentals = await repo.GetAllWithIncludesAsync(r => r.Book, r => r.Customer);

            return rentals.FirstOrDefault(r => r.Id == rentalId);
        }


    }
}
