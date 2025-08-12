using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class PublisherService: IPublisherService
    {
        private readonly ILibraryGenericRepo<Publisher> repo;

        public PublisherService(ILibraryGenericRepo<Publisher> repo, LibraryContext context)
        {
            this.repo = repo;
        }

        public async Task<Publisher> AddPublisher(Publisher publisher)
        {
            return await repo.Add(publisher);
        }

        public async Task Delete(Guid id)
        {
            var publisher = await repo.Get(id);
            repo.Delete(publisher);
        }


        public async Task<List<Publisher>> GetAllPublishers()
        {
            return await repo.GetAll();
        }

        public async Task<Publisher?> GetPublisherById(Guid id)
        {
            var publisher = await repo.Get(id);
            return publisher;
        }
        public async Task<Publisher?> Update(Publisher publisher, Publisher updatedPublisher)
        {
            publisher.Name = updatedPublisher.Name;
            publisher.Location = updatedPublisher.Location;
            publisher.LastModifiedAt= DateTime.UtcNow;
            publisher.LastModifiedBy= updatedPublisher.LastModifiedBy;

            repo.Update(publisher);
            return publisher;
        }
    }
}
