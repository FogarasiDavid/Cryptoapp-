using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IProfitService _profitSvc;
        public PortfolioController(IProfitService profitSvc)
            => _profitSvc = profitSvc;

        // GET /api/portfolio/{userId}/profit
        [HttpGet("{userId:int}/profit")]
        public async Task<ActionResult<ProfitDto>> GetTotalProfit(int userId)
        {
            var total = await _profitSvc.CalculateTotalProfitAsync(userId);
            return Ok(new ProfitDto { TotalProfit = total });
        }

        // GET /api/portfolio/{userId}/details
        [HttpGet("{userId:int}/details")]
        public async Task<ActionResult<ProfitDto>> GetProfitDetails(int userId)
        {
            var dto = await _profitSvc.GetProfitDetailsAsync(userId);
            return Ok(dto);
        }
    }
}
