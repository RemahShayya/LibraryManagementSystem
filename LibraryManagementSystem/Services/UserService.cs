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
        LibraryContext context;
        IConfiguration configuration;
        public UserService(LibraryContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<User> GetUser(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(x=>x.Username==username);
            return user;
        }

        public async Task<User> AddUser(User user)
        {
            await context.Users.AddAsync(user);
            return user;
        }

        public string HashPassword(User user, string password)
        {
            var passwordHashed = new PasswordHasher<User>()
                                .HashPassword(user, password);
            return passwordHashed;
        }

        public async Task<string> CreateJWT(User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claim,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }


        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
