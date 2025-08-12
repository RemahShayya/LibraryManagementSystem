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

        public BookService(ILibraryGenericRepo<Book> repo, LibraryContext context)
        {
            this.repo = repo;
        }

        public async Task<Book> AddBook(Book book)
        {
            return await repo.Add(book);
        }

        public async Task Delete(Guid id)
        {
            var book = await repo.Get(id);
            repo.Delete(book);
        }


        public async Task<List<Book>> GetAllBooks()
        {
            return await repo.GetAll();
        }

        public async Task<Book?> GetBookById(Guid id)
        {
            var book = await repo.Get(id);
            return book;
        }
        public async Task<Book?> Update(Book book, Book updatedBook)
        {

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Price = updatedBook.Price;
            book.LastModifiedAt = DateTime.UtcNow;
            book.LastModifiedBy = updatedBook.LastModifiedBy;
            book.PublisherId = updatedBook.PublisherId;

            repo.Update(book);
            return book;
        }

        public async Task Save(Book book)
        {
            await repo.SaveAsync();
        }
    }
}
