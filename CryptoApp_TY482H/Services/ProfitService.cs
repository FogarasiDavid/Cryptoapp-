using CryptoApp_TY482H.SqlServer;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CryptoApp_TY482H.Services
{
    public class ProfitService : IProfitService
    {
        private readonly ApplicationDbContext _db;
        public ProfitService(ApplicationDbContext db) => _db = db;

        public async Task<decimal> CalculateTotalProfitAsync(int userId)
        {
            var txs = await _db.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.Crypto)
                .ToListAsync();

            return txs.Sum(t =>
                (t.Type == "Sell" ? 1m : -1m)
              * t.Price
              * t.Amount
            );
        }

        public async Task<ProfitDto> GetProfitDetailsAsync(int userId)
        {
            var txs = await _db.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.Crypto)
                .ToListAsync();

            var details = txs
                .GroupBy(t => t.Crypto.Name)
                .Select(g =>
                {
                    var profit = g.Sum(t =>
                        (t.Type == "Sell" ? 1m : -1m)
                      * t.Price
                      * t.Amount
                    );

                    return new ProfitDetailDto
                    {
                        CryptoName = g.Key,
                        ProfitLoss = profit
                    };
                })
                .ToList();

            var total = details.Sum(d => d.ProfitLoss);

            return new ProfitDto
            {
                TotalProfit = total,
                ProfitDetails = details
            };
        }
    }
}
