using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface ICategory
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(Guid id);
        Task<Category> AddAuthor(Category category);
        Task Delete(Guid id);
        Task<Category?> Update(Category category, Category updatedAuthor);
    }
}
