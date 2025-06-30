namespace CryptoApp_TY482H.Entities
{
    public class Transactions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } //Vagy Buy vagy Sell
        public Cryptos Crypto { get; set; }
        public User User { get; set; }
    }
}
