using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.DTOs; 
using CryptoApp_TY482H.SqlServer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoApp_TY482H.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CryptoApp_TY482H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CryptosController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/cryptos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CryptoDto>>> GetCryptos()
        {
            var cryptos = await _db.Cryptos
                .Select(c => new CryptoDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CurrentPrice = c.CurrentPrice
                })
                .ToListAsync();

            return cryptos;
        }

        // GET: api/cryptos/{cryptoId}
        [HttpGet("{cryptoId}")]
        public async Task<ActionResult<CryptoDto>> GetCrypto(int cryptoId)
        {
            var crypto = await _db.Cryptos
                .Where(c => c.Id == cryptoId)
                .Select(c => new CryptoDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CurrentPrice = c.CurrentPrice
                })
                .FirstOrDefaultAsync();

            if (crypto == null)
            {
                return NotFound();
            }

            return crypto;
        }

        // POST: api/cryptos
        [HttpPost]
        public async Task<ActionResult<CryptoDto>> PostCrypto([FromBody] CryptoDto cryptoDto)
        {
            var crypto = new Cryptos
            {
                Name = cryptoDto.Name,
                CurrentPrice = cryptoDto.CurrentPrice
            };

            _db.Cryptos.Add(crypto);
            await _db.SaveChangesAsync();

            // Visszaadjuk a létrehozott kriptovalutát
            var createdCryptoDto = new CryptoDto
            {
                Id = crypto.Id,
                Name = crypto.Name,
                CurrentPrice = crypto.CurrentPrice
            };

            return CreatedAtAction(nameof(GetCrypto), new { cryptoId = crypto.Id }, createdCryptoDto);
        }

        // DELETE: api/cryptos/{cryptoId}
        [HttpDelete("{cryptoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCrypto(int cryptoId)
        {
            var crypto = await _db.Cryptos.FindAsync(cryptoId);
            if (crypto == null) return NotFound();

            _db.Cryptos.Remove(crypto);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
