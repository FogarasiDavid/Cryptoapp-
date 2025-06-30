using Microsoft.AspNetCore.Mvc;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.DTOs.Auth;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userSvc;
        private readonly ApplicationDbContext _db;

        public AuthController(IUserService userSvc, ApplicationDbContext db)
        {
            _userSvc = userSvc;
            _db = db;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterUserDto dto)
        {
            // 1) Create user entity
            var user = new User
            {
                Username = dto.UserName,
                Email = dto.Email,
                Role = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = 0m
            };
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            return CreatedAtAction(nameof(Register), new { id = user.Id }, response);
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _userSvc.LoginAsync(dto.Username, dto.Password);
            if (token == null)
                return Unauthorized(new { error = "Érvénytelen hitelesítő adatok!" });

            return Ok(new { token });
        }
    }
}
