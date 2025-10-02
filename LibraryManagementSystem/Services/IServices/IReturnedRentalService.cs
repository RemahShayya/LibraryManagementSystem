using LibraryManagementSystem.Data.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IReturnedRentalService
    {
        Task<IEnumerable<ReturnedRental>> GetAllReturnedRentals();
        Task<ReturnedRental> GetReturnedRentalById(Guid id);
        Task<ReturnedRental> AddReturnedRental(ReturnedRental returnedRental);
        Task<ReturnedRental?> GetBookReturnedRentalByIdWithIncludes(Guid returnedRentalId);
        Task<IEnumerable<ReturnedRental>> GetAllBookReturnedRentalsWithIncludes();
        void Delete(ReturnedRental returnedRental);
        Task<ReturnedRental> Update(ReturnedRental returnedRental);
        Task Save(ReturnedRental returnedRental);
    }
}
