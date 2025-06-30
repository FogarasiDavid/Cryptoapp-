namespace CryptoApp_TY482H.DTOs
{
    public class ProfitDto
    {
        public decimal TotalProfit { get; set; }
        public List<ProfitDetailDto> ProfitDetails { get; set; }
    }

    public class ProfitDetailDto
    {
        public string CryptoName { get; set; }
        public decimal ProfitLoss { get; set; }
    }
}
