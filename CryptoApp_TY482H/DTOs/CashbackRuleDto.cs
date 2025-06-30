using CryptoApp_TY482H.Entities;
namespace CryptoApp_TY482H.DTOs
{
    public record CashbackRuleDto(decimal Min, decimal? Max, decimal Percent);
}
