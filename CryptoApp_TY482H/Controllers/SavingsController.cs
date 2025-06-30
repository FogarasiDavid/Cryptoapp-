using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.SqlServer;
using CryptoApp_TY482H.Services;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SavingsController : ControllerBase
    {
        private readonly ISavingsService _svc;
        private readonly ApplicationDbContext _db;

        public SavingsController(ISavingsService svc, ApplicationDbContext db)
        {
            _svc = svc;
            _db = db;
        }

        // POST api/savings/lock
        [HttpPost("lock")]
        public async Task<IActionResult> Lock([FromBody] CreateSavingsLockDto dto)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!User.IsInRole("Admin"))
            {
                dto.UserId = me;
            }

            if (!await _db.Users.AnyAsync(u => u.Id == dto.UserId))
                return NotFound(new { error = "Felhasználó nem található" });

            if (!await _db.Cryptos.AnyAsync(c => c.Id == dto.CryptoId))
                return NotFound(new { error = "Kriptovaluta nem található" });

            var result = await _svc.CreateLockAsync(dto);
            if (result == null)
                return BadRequest(new { error = "Nincs elegendő összeg vagy invalid data." });

            return CreatedAtAction(nameof(List), new { userId = result.UserId }, result);
        }

        // GET api/savings/{userId}
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> List(int userId)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!User.IsInRole("Admin") && me != userId)
                return Forbid();

            var (active, expired) = await _svc.GetLocksAsync(userId);
            return Ok(new { Active = active, Expired = expired });
        }

        // GET api/savings/rates
        [HttpGet("rates")]
        public async Task<IActionResult> GetCurrentRates()
            => Ok(await _svc.GetCurrentRatesAsync());

        // PUT api/savings/interest-rate
        // PUT api/savings/interest-rate
        [HttpPut("interest-rate")]
        public async Task<IActionResult> UpdateRate([FromBody] InterestRateDto dto)
        {
            if (!await _db.Cryptos.AnyAsync(c => c.Id == dto.CryptoId))
                return NotFound(new { error = "Kriptovaluta nem található" });

            if (dto.Rate < 0 || dto.Rate > 3)
                return BadRequest(new { error = "Érvénytelen kamatláb. A kamatláb 0 és 3 közötti lehet." });

            await _svc.UpdateInterestRateAsync(dto.CryptoId, dto.Rate);
            return NoContent();
        }


        // DELETE api/savings/unlock/{lockId}
        [HttpDelete("unlock/{lockId:int}")]
        public async Task<IActionResult> Unlock(int lockId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var net = await _svc.EarlyUnlockAsync(userId, lockId);

            if (net == null)
                return BadRequest(new { error = "Csak a saját, aktív lekötést oldhatod fel." });

            return Ok(new { refundedAmount = net });
        }
    }
}
