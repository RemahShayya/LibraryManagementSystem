using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAuthors();
        Task<Author?> GetAuthorById(Guid id);
        Task<Author> AddAuthor(Author author);
        Task Delete(Guid id);
        Task<Author?> Update(Author author, Author updatedAuthor);
    }
}
