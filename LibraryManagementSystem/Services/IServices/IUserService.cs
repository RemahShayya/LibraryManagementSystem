using LibraryManagementSystem.Data.Identity;

namespace LibraryManagementSystem.API.Services.IServices
{
    public interface IUserService
    {
        Task<User?> GetUser(string username);
        Task<User> AddUser(User user);
        string HashPassword(User user, string password);
        Task<string> CreateJWT(User user);
        Task Save();
    }
}
