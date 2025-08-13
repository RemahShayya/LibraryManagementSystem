using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(Guid id);
        Task<Category> AddCategory(Category category);
        Task Delete(Guid id);
        Task<Category?> Update(Category category);
        Task Save(Category category);
    }
}
