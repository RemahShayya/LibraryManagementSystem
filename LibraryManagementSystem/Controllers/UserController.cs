using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.API.Services.IServices;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var existingUser = await userService.GetUser(userDto.Username);
            if (existingUser != null)
                return BadRequest("Username already taken");

            var newUser = new User
            {
                Username = userDto.Username,
            };

            newUser.HashedPassword = userService.HashPassword(newUser, userDto.Password);
            await userService.AddUser(newUser);
            await userService.Save();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDto)
        {
            var user = await userService.GetUser(userDto.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.HashedPassword, userDto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid username or password");

            var token = await userService.CreateJWT(user);

            return Ok(new { Token = token });
        }
    }
}
