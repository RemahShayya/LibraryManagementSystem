using Azure.Core;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.Data.Repositories;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.Identity;
using LibraryManagmentSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementSystem.API.Services
{
    public class UserService : IUserService
    {
        private readonly ILibraryGenericRepo<User> repo;

        public UserService(ILibraryGenericRepo<User> repo)
        {
            this.repo = repo;
        }

        public async Task<User> AddUser(User user)
        {
            return await repo.Add(user);
        }
        public void Delete(User user)
        {
            repo.Delete(user);
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await repo.GetAll();
        }
        public async Task<User?> GetUserById(string id)
        {
            var user = await repo.Get(id);
            return user;
        }
        public async Task<User?> Update(User user)
        {
            repo.Update(user);
            return user;
        }
        public async Task Save(User user)
        {
            await repo.SaveAsync();
        }

    }
}
