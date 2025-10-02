using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class AuthorImageService : IAuthorImageService
    {
        private readonly ILibraryGenericRepo<AuthorImage> repo;
        public AuthorImageService(LibraryGenericRepo<AuthorImage> repo)
        {
            this.repo = repo;
        }

        public async Task<AuthorImage> AddAuthorImage(AuthorImage authorImage)
        {
            return await repo.Add(authorImage);
        }

        public void Delete(AuthorImage authorImage)
        {
            repo.Delete(authorImage);
        }

        public async Task<IEnumerable<AuthorImage>> GetAllAuthorsImages()
        {
            return await repo.GetAll();
        }

        public async Task<AuthorImage?> GetAuthorImageById(Guid id)
        {
            var authorImage = await repo.Get(id);
            return authorImage;
        }

        public async Task<AuthorImage?> Update(AuthorImage authorImage)
        {
            repo.Update(authorImage);
            return authorImage;
        }

        public async Task Save(AuthorImage authorImage)
        {
            await repo.SaveAsync();
        }
    }
}
