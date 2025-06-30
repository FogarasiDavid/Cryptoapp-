using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.Entities;

public interface IPriceUpgradeService
{
    Task<IEnumerable<PriceUpgrade>> GetAllAsync();
    Task<IEnumerable<PriceUpgrade>> GetByCryptoIdAsync(int cryptoId);
    Task<PriceUpgrade> AddAsync(int cryptoId, decimal newPrice);
}
