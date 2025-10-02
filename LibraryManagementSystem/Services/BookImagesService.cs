using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.Entities.ImageEntities;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class BookImagesService : IBookImageService
    {
        private readonly ILibraryGenericRepo<BookImages> repo;

        public BookImagesService(ILibraryGenericRepo<BookImages> repo)
        {
            this.repo = repo;
        }

        public async Task<BookImages> AddBookImages(BookImages bookImage)
        {
            return await repo.Add(bookImage);
        }

        public void Delete(BookImages bookImage)
        {
            repo.Delete(bookImage);
        }

        public async Task<IEnumerable<BookImages>> GetAllBookImages()
        {
            return await repo.GetAll();
        }

        public async Task<BookImages?> GetBookImagesById(Guid id)
        {
            var book = await repo.Get(id);
            return book;
        }

        public async Task<BookImages?> Update(BookImages bookImage)
        {
            repo.Update(bookImage);
            return bookImage;
        }

        public async Task Save(BookImages bookImage)
        {
            await repo.SaveAsync();
        }
    }
}
