using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/trade")]
    [Authorize]
    public class LimitOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LimitOrdersController(ApplicationDbContext context)
            => _context = context;

        // POST api/trade/limit-buy
        [HttpPost("limit-buy")]
        public async Task<IActionResult> CreateLimitBuy([FromBody] CreateLimitOrderDto dto)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin) dto.UserId = me;

            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
                return NotFound(new { error = "Nincs ilyen felhasználó!" });

            if (!await _context.Cryptos.AnyAsync(c => c.Id == dto.CryptoId))
                return NotFound(new { error = "Nincs ilyen crypto!" });

            var wallet = await _context.Wallets
                .Include(w => w.WalletCryptos)
                .FirstOrDefaultAsync(w => w.UserId == dto.UserId);
            if (wallet == null)
                return NotFound(new { error = "Nincs ilyen tárca!" });

            var required = dto.LimitPrice * dto.Amount;
            if (wallet.Balance < required)
                return BadRequest(new { error = "Nincs elegendő összeg!" });

            var order = new LimitOrder
            {
                UserId = dto.UserId,
                CryptoId = dto.CryptoId,
                Amount = dto.Amount,
                LimitPrice = dto.LimitPrice,
                ExpirationTime = dto.ExpirationTime,
                Type = LimitOrderType.Buy,
                Status = LimitOrderStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.LimitOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListActive), new { userId = dto.UserId }, order);
        }

        // POST api/trade/limit-sell
        [HttpPost("limit-sell")]
        public async Task<IActionResult> CreateLimitSell([FromBody] CreateLimitOrderDto dto)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin) dto.UserId = me;

            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
                return NotFound(new { error = "Nincs ilyen felhasználó!" });

            if (!await _context.Cryptos.AnyAsync(c => c.Id == dto.CryptoId))
                return NotFound(new { error = "Nincs ilyen crypto!" });

            var wallet = await _context.Wallets
                .Include(w => w.WalletCryptos)
                .FirstOrDefaultAsync(w => w.UserId == dto.UserId);
            if (wallet == null)
                return NotFound(new { error = "Nincs ilyen Tárca!" });

            var holding = wallet.WalletCryptos
                .FirstOrDefault(wc => wc.CryptoId == dto.CryptoId);
            if (holding == null || holding.Amount < dto.Amount)
                return BadRequest(new { error = "Nincs elegendő egyenleg!" });

            var order = new LimitOrder
            {
                UserId = dto.UserId,
                CryptoId = dto.CryptoId,
                Amount = dto.Amount,
                LimitPrice = dto.LimitPrice,
                ExpirationTime = dto.ExpirationTime,
                Type = LimitOrderType.Sell,
                Status = LimitOrderStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.LimitOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListActive), new { userId = dto.UserId }, order);
        }

        // GET api/trade/limit-orders/{userId}
        [HttpGet("limit-orders/{userId:int}")]
        public async Task<IActionResult> ListActive(int userId)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && userId != me)
                return Forbid();

            var orders = await _context.LimitOrders
                .Where(o => o.UserId == userId && o.Status == LimitOrderStatus.Active)
                .ToListAsync();

            return Ok(orders);
        }

        // DELETE api/trade/limit-orders/{orderId}
        [HttpDelete("limit-orders/{orderId:int}")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var me = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");

            var order = await _context.LimitOrders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            if (!isAdmin && order.UserId != me)
                return Forbid();

            if (order.Status != LimitOrderStatus.Active)
                return BadRequest(new { error = "Csak aktív rendelések lehetnek megszüntetve!." });

            order.Status = LimitOrderStatus.Cancelled;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
