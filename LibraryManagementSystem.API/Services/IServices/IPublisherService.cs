using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IPublisherService
    {
        Task<List<Publisher>> GetAllPublishers();
        Task<Publisher?> GetPublisherById(Guid id);
        Task<Publisher> AddPublisher(Publisher publisher);
        Task Delete(Guid id);
        Task<Publisher?> Update(Publisher Publisher, Publisher updatedPublisher);
    }
}
