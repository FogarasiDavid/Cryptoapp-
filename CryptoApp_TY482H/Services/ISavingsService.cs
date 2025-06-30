using CryptoApp_TY482H.DTOs;

namespace CryptoApp_TY482H.Services
{
    public interface ISavingsService
    {
        Task<SavingsLockDto> CreateLockAsync(CreateSavingsLockDto dto);
        Task<(IEnumerable<SavingsLockDto> active, IEnumerable<SavingsLockDto> expired)> GetLocksAsync(int userId);
        Task UpdateInterestRateAsync(int cryptoId, decimal newRate);
        Task<IEnumerable<InterestRateDto>> GetCurrentRatesAsync();
        Task<decimal?> EarlyUnlockAsync(int userId, int lockId);
    }
}
