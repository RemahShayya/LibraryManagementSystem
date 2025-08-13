using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ILibraryGenericRepo<Category> repo;

        public CategoryService(ILibraryGenericRepo<Category> repo, LibraryContext context)
        {
            this.repo = repo;
        }

        public async Task<Category> AddCategory(Category category)
        {
            return await repo.Add(category);
        }

        public async Task Delete(Guid id)
        {
            var category = await repo.Get(id);
            repo.Delete(category);
        }


        public async Task<List<Category>> GetAllCategories()
        {
            return await repo.GetAll();
        }

        public async Task<Category?> GetCategoryById(Guid id)
        {
            var book = await repo.Get(id);
            return book;
        }
        public async Task<Category?> Update(Category category)
        {
            repo.Update(category);
            return category;
        }

        public async Task Save(Category category)
        {
            await repo.SaveAsync();
        }
    }
}
