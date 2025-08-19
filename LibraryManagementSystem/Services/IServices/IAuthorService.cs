using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author?> GetAuthorById(Guid id);
        Task<Author> AddAuthor(Author author);
        void Delete(Author author);
        Task<Author?> Update(Author author);
        Task Save(Author author);
    }
}
