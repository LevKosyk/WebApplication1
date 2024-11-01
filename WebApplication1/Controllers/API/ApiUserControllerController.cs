using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public APIUserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register( string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email or password are error!");
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true 
            };
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                return Ok("User is registered successfully ...");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Auth( string email, string password)
        {
            Console.WriteLine($"Login: {email}, password: {password}");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email or password are error!");
            }
            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: false,
                lockoutOnFailure: false
            );
            if (result.Succeeded)
            {
                return Ok("The user is authorized ...");
            }
            return BadRequest("Invalid email or password ...");
        }

        [HttpPost("access-denied")]
        public IActionResult AccessDenied()
        {
            return BadRequest("Access Denied");
        }
    }
}
