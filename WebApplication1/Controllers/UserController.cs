using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
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
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Error register!");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
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
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Error auth ...");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("The role name is important ...");
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("The role name is already  exists ...");
            }
            var role = new IdentityRole
            {
                Name = roleName
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok("The role is created ...");
            }
            return BadRequest(Json(result.Errors));
        }
        [HttpGet]
        public IActionResult AssignRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("User ID or role name cannot be empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return BadRequest("The role does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok("Role assigned successfully.");
            }

            return BadRequest(result.Errors);
        }
    }
}
