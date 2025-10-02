using LibraryManagementSystem.Data.Entities.ImageEntities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IBookImageService
    {
        Task<IEnumerable<BookImages>> GetAllBookImages();
        Task<BookImages?> GetBookImagesById(Guid id);
        Task<BookImages> AddBookImages(BookImages bookImage);
        void Delete(BookImages bookImage);
        Task<BookImages?> Update(BookImages bookImage);
        Task Save(BookImages bookImage);
    }
}
