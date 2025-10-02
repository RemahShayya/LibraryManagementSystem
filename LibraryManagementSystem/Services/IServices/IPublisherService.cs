using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishers();
        Task<Publisher?> GetPublisherById(Guid id);
        Task<Publisher> AddPublisher(Publisher publisher);
        void Delete(Publisher publisher);
        Task<Publisher?> Update(Publisher Publisher);
        Task Save(Publisher publisher);
    }
}
