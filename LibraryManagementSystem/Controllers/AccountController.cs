using LibraryManagementSystem.API.DTO;
using LibraryManagementSystem.API.DTO.Requests;
using LibraryManagementSystem.API.Services;
using LibraryManagementSystem.Data.DTO;
using LibraryManagementSystem.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService jwtService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        public readonly EmailService emailService;
        private readonly IConfiguration configuration;

        public AccountController(JWTService jwtService, SignInManager<User> signInManager, UserManager<User> userManager, EmailService emailService, IConfiguration configuration)
        {
            this.jwtService = jwtService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> login(CreateLoginRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null) return Unauthorized("Invalid Username or Password");
            if (user.EmailConfirmed == false) return Unauthorized("Please confirm your email!");
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Username or Password!");

            return await CreateApplicationUserDTO(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreatedRegisterRequest request)
        {

            var checkEmail = await CheckIfEmailExist(request.Email);
            if (checkEmail)
                return Unauthorized($"An existing account is using {request.Email}");

            // Validate role before creating user
            var allowedRoles = new List<string> { "Admin", "Customer" };
            if (!allowedRoles.Contains(request.Role))
            {
                return BadRequest($"Invalid role: {request.Role}");
            }

            var addedUser = new User()
            {
                FirstName = request.Firstname.ToLower(),
                LastName = request.Lastname.ToLower(),
                Email = request.Email.ToLower(),
                UserName = request.Email.ToLower()
            };

            var result = await userManager.CreateAsync(addedUser, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleToAssign = request.Role == "Admin" ? "Admin" : "Customer";
            await userManager.AddToRoleAsync(addedUser, roleToAssign);

            try
            {
                if (await SendConfirmationEmailAsync(addedUser))
                {
                    return Ok(new
                    {
                        title = "Account Created",
                        message = "Your account has been created, you can login, please confirm your email address"
                    });
                }
                return BadRequest("Failed to send email!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email!");
            }
        }


        [HttpPut("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return BadRequest("Email has not been registered yet!");
            if (user.EmailConfirmed == true) return BadRequest("Your email address is already confirmed, please login to your account");

            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
                var result = await userManager.ConfirmEmailAsync(user, decodedToken);

                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Email confirmed", message = "Your email has been confirmed" }));
                }
                return BadRequest("Email could not be confirmed!");
            }
            catch (Exception)
            {
                return BadRequest("Email could not be confirmed!");
            }
        }

        //[Authorize]
        //[HttpPost("refresh-user-token")]
        //public async Task<ActionResult<UserDTO>> RefreshUserToken()
        //{
        //    var user = await userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //    return CreateApplicationUserDTO(user);
        //}

        [HttpPost("ResendEmailConfirmationLink/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email!");
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized("This email address hasn't been registered yet!");
            if (user.EmailConfirmed == true) return BadRequest("Your email address is already confirmed, please login to your account");

            try
            {
                if (await SendConfirmationEmailAsync(user))
                {
                    return Ok(new JsonResult(new { title = "Confirmed link sent", message = "Please confirm your email address" }));
                }
                return BadRequest("Failed to send email!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email!");
            }
        }
        
        [HttpPost("ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotUsernameOrPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email!");
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized("This email address hasn't been registered yet");
            if (user.EmailConfirmed == false) return Unauthorized("Please confirm your email first!");

            try
            {
                if (await SendForgotUsernameOrPasswordEmail(user))
                {
                    return Ok(new JsonResult(new { title = "Reset link sent", message = "Please check your email to reset your username or password" }));
                }
                return BadRequest("Failed to send email!");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email!");

            }
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if(user == null) return BadRequest("Email has not been registered yet!");
            if(user.EmailConfirmed == false) return Unauthorized("Please confirm your email first!");

            try
            {
                var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Password reset succeeded", message = "Password have been reset" }));
                }
                return BadRequest("Password could not be reset!");
            }
            catch (Exception)
            {
                return BadRequest("Password could not be reset!");
            }
        }

        #region
        private async Task<UserDTO> CreateApplicationUserDTO(User user)
        {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = await jwtService.CreateJWT(user)
            };
        }

        private async Task<bool> CheckIfEmailExist(string email)
        {
            return await userManager.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        private async Task<bool> SendConfirmationEmailAsync(User user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{configuration["JWT:ClientURL"]}/{configuration["Email:ConfirmationEmailPath"]}?token={encodedToken}&email={user.Email}";
            var body = $"<p>Hello:{user.FirstName}</p>" +
                "<p>Please confirm your email address by clicking on the following link.</p>" +
                $"<p><a href=\"{confirmationLink}\">Click here</a></p>" +
                "<p>Thank You!</p>" +
                $"<br><p>{configuration["Email:ApplicationName"]}</p>";
            var emailSent = new SendEmailDTO(user.Email, "Confirm your email!", body);

            return await emailService.SendEmailAsync(emailSent);
        }

        private async Task<bool> SendForgotUsernameOrPasswordEmail(User user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{configuration["JWT:ClientURL"]}/{configuration["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";
            var body = $"<p>Hello:{user.FirstName}</p>" +
                "<p>Please reset your username or password by clicking on the following link.</p>" +
                $"<p><a href=\"{resetLink}\">Click here</a></p>" +
                "<p>Thank You!</p>" +
                $"<br><p>{configuration["Email:ApplicationName"]}</p>";
            var emailSent = new SendEmailDTO(user.Email, "Reset your username or password!", body);
            return await emailService.SendEmailAsync(emailSent);
        }
        #endregion
    }
}
