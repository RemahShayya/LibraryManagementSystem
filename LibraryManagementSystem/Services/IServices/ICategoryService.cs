using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(Guid id);
        Task<Category> AddCategory(Category category);
        void Delete(Category category);
        Task<Category?> Update(Category category);
        Task Save(Category category);
    }
}
