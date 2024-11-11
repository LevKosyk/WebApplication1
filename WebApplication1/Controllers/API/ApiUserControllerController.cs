using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Services;

namespace WebApplication1.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration  _configuration;
        public APIUserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] WebApplication1.Models.RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email or password are error!");
            }
            var newUser = new IdentityUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                EmailConfirmed = true 
            };
            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (result.Succeeded)
            {
                return Ok("User is registered successfully ...");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] WebApplication1.Models.LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email or password are error!");
            }
            var user= await _userManager.FindByEmailAsync(loginModel.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (result.Succeeded)
            {
                ServiceUser.Id = user.Id;
                var token = GenerateJwtToken(user);
                return Ok(token);
            }
            return BadRequest("Invalid email or password ...");
        }

        [HttpPost("access-denied")]
        public IActionResult AccessDenied()
        {
            return BadRequest("Access Denied");
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();  // Создание обработчика для токенов
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);  // Получение ключа из конфигурации и его кодирование в байты

            var tokenDescriptor = new SecurityTokenDescriptor  // Определение параметров токена (описание токена)
            {
                Subject = new ClaimsIdentity(new[]  // Добавление утверждений (claims) о пользователе в токен
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),  // Утверждение: идентификатор пользователя
                    new Claim(ClaimTypes.Name, user.UserName),  // Утверждение: имя пользователя
                    new Claim(ClaimTypes.Email, user.Email)  // Утверждение: email пользователя
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),  // Установка срока действия токена
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),  // Подпись токена с использованием симметричного ключа и алгоритма HMAC SHA256 (потрібен пароль у appsettings.json мінімум 32 символи)
                Issuer = _configuration["Jwt:Issuer"],  // Установка издателя токена (опционально)
                Audience = _configuration["Jwt:Audience"]  // Установка аудитории токена (опционально)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);  // Создание токена на основе описания
            return tokenHandler.WriteToken(token);  // Возврат сгенерированного токена в строковом формате
        }
    }
}
