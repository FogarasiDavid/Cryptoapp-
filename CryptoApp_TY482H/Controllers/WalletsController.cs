using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.SqlServer;
using CryptoApp_TY482H.Entities;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public WalletController(ApplicationDbContext db) => _db = db;

        // GET api/wallet/health
        [AllowAnonymous]
        [HttpGet("health")]
        public IActionResult Health()
        {
            var count = _db.Wallets.Count();
            return Ok(new { wallets = count });
        }

        // GET api/wallet/{userId}
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetWallet(int userId)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && me != userId)
                return Forbid();

            var wallet = await _db.Wallets
                .Include(w => w.WalletCryptos)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                if (isAdmin)
                {
                    return NotFound(new { error = "Wallet not found." });
                }

                wallet = new Wallet { UserId = userId, Balance = 0m };
                _db.Wallets.Add(wallet);
                await _db.SaveChangesAsync();

                wallet = await _db.Wallets
                    .Include(w => w.WalletCryptos)
                    .FirstAsync(w => w.UserId == userId);
            }

            var dto = new WalletDto
            {
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                WalletCryptos = wallet.WalletCryptos
                    .Select(wc => new WalletCryptoDto
                    {
                        CryptoId = wc.CryptoId,
                        Amount = wc.Amount
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        // PUT api/wallet/{userId}
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateBalance(int userId, [FromBody] decimal newBalance)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && me != userId)
                return Forbid();

            var wallet = await _db.Wallets
                .Include(w => w.WalletCryptos)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return NotFound(new { error = "Wallet not found." });

            wallet.Balance = newBalance;
            await _db.SaveChangesAsync();

            var dto = new WalletDto
            {
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                WalletCryptos = wallet.WalletCryptos
                    .Select(wc => new WalletCryptoDto
                    {
                        CryptoId = wc.CryptoId,
                        Amount = wc.Amount
                    })
                    .ToList()
            };
            return Ok(dto);
        }

        // DELETE api/wallet/{userId}
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteWallet(int userId)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && me != userId)
                return Forbid();

            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return NotFound(new { error = "Wallet not found." });

            _db.Wallets.Remove(wallet);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
