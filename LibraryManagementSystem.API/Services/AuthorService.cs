using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.API.Services.IServices
{
    public class AuthorService : IAuthorService
    {
        private readonly ILibraryGenericRepo<Author> repo;

        public AuthorService(ILibraryGenericRepo<Author> repo, LibraryContext context)
        {
            this.repo = repo;
        }
        public async Task<Author> AddAuthor(Author author)
        {
            return await repo.Add(author);
        }

        public async Task Delete(Guid id)
        {
            var author = await repo.Get(id);
            repo.Delete(author);
        }


        public async Task<List<Author>> GetAllAuthors()
        {
            return await repo.GetAll();
        }
        
        public async Task<Author?> GetAuthorById(Guid id)
        {
            var author = await repo.Get(id);
            return author;
        }
        public async Task<Author?> Update(Author author)
        {
            repo.Update(author);
            return author;
        }

        public async Task Save(Author author)
        {
            await repo.SaveAsync();
        }
    }
}
