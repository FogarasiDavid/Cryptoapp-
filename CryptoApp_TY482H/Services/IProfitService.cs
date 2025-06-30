using CryptoApp_TY482H.DTOs;
using System.Threading.Tasks;

namespace CryptoApp_TY482H.Services.Interfaces
{
    public interface IProfitService
    {
        Task<decimal> CalculateTotalProfitAsync(int userId);
        Task<ProfitDto> GetProfitDetailsAsync(int userId);
    }
}
