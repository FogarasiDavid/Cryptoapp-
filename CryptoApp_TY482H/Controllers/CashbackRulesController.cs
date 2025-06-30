using Microsoft.AspNetCore.Mvc;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Services;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashbackRulesController : ControllerBase
    {
        private readonly ICashbackService _svc;
        public CashbackRulesController(ICashbackService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _svc.GetRulesAsync());

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CashbackRuleDto[] rules)
        {
            await _svc.UpdateRulesAsync(rules);
            return NoContent();
        }
    }
}
