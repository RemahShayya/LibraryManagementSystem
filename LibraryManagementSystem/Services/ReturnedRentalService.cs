namespace LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReturnedRentalService : IReturnedRentalService
{
    private readonly ILibraryGenericRepo<ReturnedRental> libraryGenericRepo;

    public ReturnedRentalService(ILibraryGenericRepo<ReturnedRental> libraryGenericRepo)
    {
        this.libraryGenericRepo = libraryGenericRepo;
    }

    public async Task<ReturnedRental> AddReturnedRental(ReturnedRental returnedRental)
    {
        return await libraryGenericRepo.Add(returnedRental);
    }

    public void Delete(ReturnedRental returnedRental)
    {
        libraryGenericRepo.Delete(returnedRental);
    }

    public async Task<IEnumerable<ReturnedRental>> GetAllReturnedRentals()
    {
        return await libraryGenericRepo.GetAll();
    }

    public Task<ReturnedRental> GetReturnedRentalById(Guid id)
    {
        return libraryGenericRepo.Get(id);
    }

    public async Task<IEnumerable<ReturnedRental>> GetAllBookReturnedRentalsWithIncludes()
    {
        return await libraryGenericRepo.GetAllWithIncludesAsync(
            r => r.Book,
            r => r.Customer
        );
    }

    public async Task<ReturnedRental?> GetBookReturnedRentalByIdWithIncludes(Guid returnedRentalId)
    {
        var rentals = await libraryGenericRepo.GetAllWithIncludesAsync(r => r.Book, r => r.Customer);

        return rentals.FirstOrDefault(r => r.Id== returnedRentalId);
    }

    public Task Save(ReturnedRental returnedRental)
    {
        return libraryGenericRepo.SaveAsync();
    }

    public async Task<ReturnedRental> Update(ReturnedRental returnedRental)
    {
        return libraryGenericRepo.Update(returnedRental);
    }
}
