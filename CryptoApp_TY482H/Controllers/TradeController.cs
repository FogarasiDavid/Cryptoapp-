using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.DTOs.Trade;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/trade")]
    [Authorize]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeSvc;
        public TradeController(ITradeService tradeSvc) => _tradeSvc = tradeSvc;

        // POST /api/trade/buy
        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] BuyDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _tradeSvc.BuyAsync(userId, dto);
            if (result == null)
                return BadRequest(new { error = "Elégtelen egyenleg vagy érvénytelen adatok" });
            return Ok(result);
        }

        // POST /api/trade/sell
        [HttpPost("sell")]
        public async Task<IActionResult> Sell([FromBody] SellDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _tradeSvc.SellAsync(userId, dto);
            if (result == null)
                return BadRequest(new { error = "Elégtelen kripto vagy érvénytelen adatok" });
            return Ok(result);
        }

        // GET /api/trade/portfolio
        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var port = await _tradeSvc.GetPortfolioAsync(userId);
            return Ok(port);
        }
    }
}
