using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book?> GetBookById(Guid id);
        Task<Book> AddBook(Book book);
        void Delete(Book book);
        Task<Book?> Update(Book book);
        Task Save(Book book);
    }
}
