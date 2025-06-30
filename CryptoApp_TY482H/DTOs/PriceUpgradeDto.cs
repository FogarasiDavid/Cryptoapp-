namespace CryptoApp_TY482H.DTOs
{
    public class PriceUpgradeDto
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime UpgradeDate { get; set; }
    }
}
