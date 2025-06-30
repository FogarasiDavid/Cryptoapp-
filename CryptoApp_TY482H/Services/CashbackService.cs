using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;

namespace CryptoApp_TY482H.Services
{
    public class CashbackService : ICashbackService
    {
        private readonly ApplicationDbContext _db;
        public CashbackService(ApplicationDbContext db) => _db = db;

        public async Task<decimal> CalculateCashbackAsync(int userId, int cryptoId, decimal amount, decimal total)
        {
            var rules = await _db.CashbackRules.OrderBy(r => r.MinAmount).ToListAsync();
            var rule = rules.Where(r => total >= r.MinAmount && (r.MaxAmount == null || total <= r.MaxAmount))
                            .LastOrDefault();

            var cashback = rule != null
                ? Math.Round(total * rule.Percent / 100m, 2)
                : 0m;

            var wallet = await _db.Wallets.FirstAsync(w => w.UserId == userId);
            wallet.Balance += cashback;
            await _db.SaveChangesAsync();

            return cashback;
        }

        public async Task<IReadOnlyList<CashbackRuleDto>> GetRulesAsync()
            => await _db.CashbackRules
                        .Select(r => new CashbackRuleDto(r.MinAmount, r.MaxAmount, r.Percent))
                        .ToListAsync();

        public async Task UpdateRulesAsync(IEnumerable<CashbackRuleDto> dtos)
        {
            _db.CashbackRules.RemoveRange(_db.CashbackRules);
            await _db.SaveChangesAsync();

            await _db.CashbackRules.AddRangeAsync(
                dtos.Select(d => new CashbackRule
                {
                    MinAmount = d.Min,
                    MaxAmount = d.Max,
                    Percent = d.Percent
                })
            );
            await _db.SaveChangesAsync();
        }
    }
}
