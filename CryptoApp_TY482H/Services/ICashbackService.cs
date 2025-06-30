using CryptoApp_TY482H.DTOs;

namespace CryptoApp_TY482H.Services
{
    public interface ICashbackService
    {
        Task<decimal> CalculateCashbackAsync(int userId, int cryptoId, decimal amount, decimal total);
        Task<IReadOnlyList<CashbackRuleDto>> GetRulesAsync();
        Task UpdateRulesAsync(IEnumerable<CashbackRuleDto> rules);
    }
}
