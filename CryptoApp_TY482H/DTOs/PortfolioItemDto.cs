namespace CryptoApp_TY482H.DTOs.Trade
{
    public class PortfolioItemDto
    {
        public decimal Balance { get; set; }
        public List<HoldingDto> Holdings { get; set; }
    }

    public class HoldingDto
    {
        public int CryptoId { get; set; }
        public string CryptoName { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentPrice { get; set; }
    }

}