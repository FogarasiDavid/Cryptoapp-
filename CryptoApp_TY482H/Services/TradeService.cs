using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.DTOs.Trade;
using CryptoApp_TY482H.SqlServer;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Services.Interfaces;

namespace CryptoApp_TY482H.Services
{
    public class TradeService : ITradeService
    {
        private readonly ApplicationDbContext _db;
        public TradeService(ApplicationDbContext db) => _db = db;

        public async Task<TradeResponseDto?> BuyAsync(int userId, BuyDto dto)
        {
            var user = await _db.Users
                  .Include(u => u.Wallet).ThenInclude(w => w.WalletCryptos)
                  .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            if (user.Wallet == null)
            {
                user.Wallet = new Wallet { UserId = userId, Balance = 0m };
                _db.Wallets.Add(user.Wallet);
                await _db.SaveChangesAsync();
            }

            var crypto = await _db.Cryptos.FindAsync(dto.CryptoId);
            if (crypto == null)
                return null;

            var total = crypto.CurrentPrice * dto.Amount;
            if (user.Wallet.Balance < total)
                return null;

            user.Wallet.Balance -= total;

            var wc = user.Wallet.WalletCryptos.FirstOrDefault(x => x.CryptoId == dto.CryptoId);
            if (wc == null)
                user.Wallet.WalletCryptos.Add(new CryptoWallet { CryptoId = dto.CryptoId, Amount = dto.Amount });
            else
                wc.Amount += dto.Amount;

            var tx = new Transactions
            {
                UserId = userId,
                CryptoId = dto.CryptoId,
                Price = crypto.CurrentPrice,
                Amount = dto.Amount,
                Timestamp = DateTime.UtcNow,
                Type = "Buy"
            };
            _db.Transactions.Add(tx);

            await _db.SaveChangesAsync();

            // calculate cashback
            var rules = await _db.CashbackRules
                                 .OrderBy(r => r.MinAmount)
                                 .ToListAsync();
            var rule = rules
                .Where(r => total >= r.MinAmount && (r.MaxAmount == null || total <= r.MaxAmount))
                .LastOrDefault();
            var cashback = rule is null
                ? 0m
                : Math.Round(total * rule.Percent / 100m, 2);

            user.Wallet.Balance += cashback;
            await _db.SaveChangesAsync();

            return new TradeResponseDto
            {
                TransactionId = tx.Id,
                UserId = userId,
                CryptoId = dto.CryptoId,
                Amount = dto.Amount,
                TotalAmount = total,
                Cashback = cashback,
                Timestamp = tx.Timestamp
            };
        }

        public async Task<PortfolioItemDto> GetPortfolioAsync(int userId)
        {
            var wallet = await _db.Wallets
                .Include(w => w.WalletCryptos).ThenInclude(x => x.Crypto)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
                return new PortfolioItemDto { Balance = 0, Holdings = new List<HoldingDto>() };

            var holdings = wallet.WalletCryptos.Select(wc => new HoldingDto
            {
                CryptoId = wc.CryptoId,
                CryptoName = wc.Crypto.Name,
                Amount = wc.Amount,
                CurrentPrice = wc.Crypto.CurrentPrice
            }).ToList();

            return new PortfolioItemDto
            {
                Balance = wallet.Balance,
                Holdings = holdings
            };
        }

        public async Task<TradeResponseDto?> SellAsync(int userId, SellDto dto)
        {
            var user = await _db.Users
                .Include(u => u.Wallet).ThenInclude(w => w.WalletCryptos)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            if (user.Wallet == null)
            {
                user.Wallet = new Wallet { UserId = userId, Balance = 0m };
                _db.Wallets.Add(user.Wallet);
                await _db.SaveChangesAsync();
            }

            var crypto = await _db.Cryptos.FindAsync(dto.CryptoId);
            if (crypto == null)
                return null;

            var wc = user.Wallet.WalletCryptos.FirstOrDefault(x => x.CryptoId == dto.CryptoId);
            if (wc == null || wc.Amount < dto.Amount)
                return null;

            wc.Amount -= dto.Amount;
            var total = crypto.CurrentPrice * dto.Amount;
            user.Wallet.Balance += total;

            var tx = new Transactions
            {
                UserId = userId,
                CryptoId = dto.CryptoId,
                Price = crypto.CurrentPrice,
                Amount = dto.Amount,
                Timestamp = DateTime.UtcNow,
                Type = "Sell"
            };
            _db.Transactions.Add(tx);

            await _db.SaveChangesAsync();

            return new TradeResponseDto
            {
                TransactionId = tx.Id,
                UserId = userId,
                CryptoId = dto.CryptoId,
                Amount = dto.Amount,
                TotalAmount = total,
                Cashback = 0m,
                Timestamp = tx.Timestamp
            };
        }
    }
}
