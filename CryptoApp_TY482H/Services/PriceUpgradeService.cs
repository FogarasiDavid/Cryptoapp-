using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;
using Microsoft.EntityFrameworkCore;

public class PriceUpgradeService : IPriceUpgradeService
{
    private readonly ApplicationDbContext _db;

    public PriceUpgradeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PriceUpgrade>> GetAllAsync()
    {
        return await _db.PriceUpgrades
                        .Include(p => p.Crypto)
                        .ToListAsync();
    }

    public async Task<IEnumerable<PriceUpgrade>> GetByCryptoIdAsync(int cryptoId)
    {
        return await _db.PriceUpgrades
                        .Where(p => p.CryptoId == cryptoId)
                        .Include(p => p.Crypto)
                        .ToListAsync();
    }

    public async Task<PriceUpgrade?> AddAsync(int cryptoId, decimal newPrice)
    {
        var exists = await _db.Cryptos.AnyAsync(c => c.Id == cryptoId);
        if (!exists)
            return null; 

        var upgrade = new PriceUpgrade
        {
            CryptoId = cryptoId,
            NewPrice = newPrice,
            UpgradeDate = DateTime.UtcNow
        };

        _db.PriceUpgrades.Add(upgrade);
        await _db.SaveChangesAsync();

        await _db.Entry(upgrade).Reference(u => u.Crypto).LoadAsync();

        return upgrade;
    }
}
