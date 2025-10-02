using LibraryManagementSystem.Data.Identity;
using LibraryManagmentSystem.Entities;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserById(string id);
        Task<User> AddUser(User user);
        void Delete(User user);
        Task<User?> Update(User user);
        Task Save(User user);
    }
}
