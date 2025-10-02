using LibraryManagementSystem.Data.Entities.ImageEntities;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IAuthorImageService
    {
        Task<IEnumerable<AuthorImage>> GetAllAuthorsImages();
        Task<AuthorImage?> GetAuthorImageById(Guid id);
        Task<AuthorImage> AddAuthorImage(AuthorImage authorImage);
        void Delete(AuthorImage authorImage);
        Task<AuthorImage> Update(AuthorImage authorImage);
        Task Save(AuthorImage authorImage);
    }
}
