using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagmentSystem.Data;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ILibraryGenericRepo<Category> repo;

        public CategoryService(ILibraryGenericRepo<Category> repo)
        {
            this.repo = repo;
        }

        public async Task<Category> AddCategory(Category category)
        {
            return await repo.Add(category);

        }
        public void Delete(Category category)
        {
            repo.Delete(category);
        }


        public async Task<IEnumerable<Category>> GetAllCategories()
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
