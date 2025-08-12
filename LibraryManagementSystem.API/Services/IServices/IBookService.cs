using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooks();
        Task<Book?> GetBookById(Guid id);
        Task<Book> AddBook(Book book);
        Task Delete(Guid id);
        Task<Book?> Update(Book book, Book updatedBook);
        Task Save(Book book);
    }
}
