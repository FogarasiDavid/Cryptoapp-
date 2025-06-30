using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp_TY482H.Services
{
    public class SavingsService : ISavingsService
    {
        private readonly ApplicationDbContext _db;
        public SavingsService(ApplicationDbContext db) => _db = db;

        public async Task<SavingsLockDto> CreateLockAsync(CreateSavingsLockDto dto)
        {
            var rate = await _db.InterestRates
                .Where(ir => ir.CryptoId == dto.CryptoId)
                .OrderByDescending(ir => ir.EffectiveDate)
                .FirstAsync();

            var sl = new SavingsLock
            {
                UserId = dto.UserId,
                CryptoId = dto.CryptoId,
                Amount = dto.Amount,
                StartDate = DateTime.UtcNow,
                DurationDays = dto.DurationDays,
                InterestRateAtLock = rate.Rate,
                Status = SavingsStatus.Active
            };
            _db.SavingsLocks.Add(sl);
            await _db.SaveChangesAsync();

            return new SavingsLockDto
            {
                Id = sl.Id,
                Amount = sl.Amount,
                CryptoId = sl.CryptoId,
                StartDate = sl.StartDate,
                DurationDays = sl.DurationDays,
                InterestRateAtLock = sl.InterestRateAtLock,
                Status = sl.Status,
                MatureAmount = sl.ComputeMatureAmount()
            };
        }

        public async Task<(IEnumerable<SavingsLockDto> active, IEnumerable<SavingsLockDto> expired)> GetLocksAsync(int userId)
        {
            var all = await _db.SavingsLocks
                .Where(sl => sl.UserId == userId)
                .Include(sl => sl.Crypto)
                .ToListAsync();

            var active = all.Where(sl => sl.Status == SavingsStatus.Active)
                .Select(sl => new SavingsLockDto
                {
                    Id = sl.Id,
                    Amount = sl.Amount,
                    CryptoId = sl.CryptoId,
                    StartDate = sl.StartDate,
                    DurationDays = sl.DurationDays,
                    InterestRateAtLock = sl.InterestRateAtLock,
                    Status = sl.Status,
                    MatureAmount = sl.ComputeAccruedUntil(DateTime.UtcNow)
                });

            var expired = all.Where(sl => sl.Status != SavingsStatus.Active)
                .Select(sl => new SavingsLockDto
                {
                    Id = sl.Id,
                    Amount = sl.Amount,
                    CryptoId = sl.CryptoId,
                    StartDate = sl.StartDate,
                    DurationDays = sl.DurationDays,
                    InterestRateAtLock = sl.InterestRateAtLock,
                    Status = sl.Status,
                    MatureAmount = sl.ComputeMatureAmount()
                });

            return (active, expired);
        }

        public async Task UpdateInterestRateAsync(int cryptoId, decimal newRate)
        {
            _db.InterestRates.Add(new InterestRate
            {
                CryptoId = cryptoId,
                Rate = newRate,
                EffectiveDate = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<InterestRateDto>> GetCurrentRatesAsync()
        {
            var maxDates = _db.InterestRates
                .GroupBy(ir => ir.CryptoId)
                .Select(g => new {
                    CryptoId = g.Key,
                    EffectiveDate = g.Max(x => x.EffectiveDate)
                });

            var query =
                from md in maxDates
                join ir in _db.InterestRates
                    on new { md.CryptoId, md.EffectiveDate }
                    equals new { ir.CryptoId, ir.EffectiveDate }
                select new InterestRateDto
                {
                    CryptoId = ir.CryptoId,
                    Rate = ir.Rate,
                    EffectiveDate = ir.EffectiveDate
                };

            return await query.ToListAsync();
        }


        public async Task<decimal?> EarlyUnlockAsync(int userId, int lockId)
        {
            var sl = await _db.SavingsLocks.FirstOrDefaultAsync(s => s.Id == lockId);
            if (sl == null || sl.UserId != userId || sl.Status != SavingsStatus.Active)
                return null;

            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return null;

            var now = DateTime.UtcNow;
            var accrued = sl.ComputeAccruedUntil(now);
            var interest = accrued - sl.Amount;
            var net = accrued - interest * 0.15m;

            wallet.Balance += net;

            sl.Status = SavingsStatus.Cancelled;

            await _db.SaveChangesAsync();

            return net;
        }

    }
}
