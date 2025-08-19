using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class PublisherService: IPublisherService
    {
        private readonly ILibraryGenericRepo<Publisher> repo;

        public PublisherService(ILibraryGenericRepo<Publisher> repo)
        {
            this.repo = repo;
        }

        public async Task<Publisher> AddPublisher(Publisher publisher)
        {
            return await repo.Add(publisher);
        }

        public void Delete(Publisher publisher)
        {
            repo.Delete(publisher);
        }
        public async Task<IEnumerable<Publisher>> GetAllPublishers()
        {
            return await repo.GetAll();
        }

        public async Task<Publisher?> GetPublisherById(Guid id)
        {
            var publisher = await repo.Get(id);
            return publisher;
        }
        public async Task<Publisher?> Update(Publisher publisher)
        {
            repo.Update(publisher);
            return publisher;
        }

        public async Task Save(Publisher publisher)
        {
            await repo.SaveAsync();
        }
    }
}
