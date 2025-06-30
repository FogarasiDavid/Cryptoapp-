using System.Threading.Tasks;
using CryptoApp_TY482H.DTOs;
using CryptoApp_TY482H.DTOs.Trade;

namespace CryptoApp_TY482H.Services.Interfaces
{
    public interface ITradeService
    {
        Task<TradeResponseDto?> BuyAsync(int userId, BuyDto dto);
        Task<TradeResponseDto?> SellAsync(int userId, SellDto dto);
        Task<PortfolioItemDto> GetPortfolioAsync(int userId);
    }
}
