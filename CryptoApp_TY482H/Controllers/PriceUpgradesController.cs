using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.Services.Interfaces;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceUpgradesController : ControllerBase
    {
        private readonly IPriceUpgradeService _svc;

        public PriceUpgradesController(IPriceUpgradeService svc)
        {
            _svc = svc;
        }

        // GET: /api/PriceUpgrades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceUpgradeDto>>> GetAll()
        {
            var list = await _svc.GetAllAsync();
            var dtos = list.Select(p => new PriceUpgradeDto
            {
                Id = p.Id,
                CryptoId = p.CryptoId,
                NewPrice = p.NewPrice,
                UpgradeDate = p.UpgradeDate
            }).ToList();

            return Ok(dtos);
        }

        // GET: /api/PriceUpgrades/by-crypto/{cryptoId}
        [HttpGet("by-crypto/{cryptoId:int}")]
        public async Task<ActionResult<IEnumerable<PriceUpgradeDto>>> GetByCrypto(int cryptoId)
        {
            var list = await _svc.GetByCryptoIdAsync(cryptoId);
            var dtos = list.Select(p => new PriceUpgradeDto
            {
                Id = p.Id,
                CryptoId = p.CryptoId,
                NewPrice = p.NewPrice,
                UpgradeDate = p.UpgradeDate
            }).ToList();

            return Ok(dtos);
        }

        // POST: /api/PriceUpgrades
        [HttpPost]
        public async Task<ActionResult<PriceUpgradeDto>> Add([FromBody] PriceUpgradeDto dto)
        {
            var created = await _svc.AddAsync(dto.CryptoId, dto.NewPrice);
            if (created == null)
            {
                return NotFound(new { error = "Nincs ilyen Crypto" });
            }

            var resultDto = new PriceUpgradeDto
            {
                Id = created.Id,
                CryptoId = created.CryptoId,
                NewPrice = created.NewPrice,
                UpgradeDate = created.UpgradeDate
            };

            return CreatedAtAction(
                nameof(GetByCrypto),
                new { cryptoId = resultDto.CryptoId },
                resultDto
            );
        }
    }
}
