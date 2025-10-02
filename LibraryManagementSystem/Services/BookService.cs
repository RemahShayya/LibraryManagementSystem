using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.API.Services
{
    public class BookService : IBookService
    {
        private readonly ILibraryGenericRepo<Book> repo;

        public BookService(ILibraryGenericRepo<Book> repo)
        {
            this.repo = repo;
        }

        public async Task<Book> AddBook(Book book)
        {
            return await repo.Add(book);
        }

        public void Delete(Book book)
        {
            repo.Delete(book);
        }


        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await repo.GetAll();
        }

        public async Task<Book?> GetBookById(Guid id)
        {
            var book = await repo.Get(id);
            return book;
        }
        public async Task<Book?> Update(Book book)
        {
            repo.Update(book);
            return book;
        }

        public async Task Save(Book book)
        {
            await repo.SaveAsync();
        }
    }
}
