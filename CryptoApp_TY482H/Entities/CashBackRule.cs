namespace CryptoApp_TY482H.Entities
{
    public class CashbackRule
    {
        public int Id { get; set; }
        public decimal MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public decimal Percent { get; set; }
    }
}
