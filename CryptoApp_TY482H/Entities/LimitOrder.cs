namespace CryptoApp_TY482H.Entities
{
    public enum LimitOrderType { Buy, Sell }
    public enum LimitOrderStatus { Active, Executed, Cancelled, Expired }

    public class LimitOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public decimal LimitPrice { get; set; }
        public DateTime ExpirationTime { get; set; }
        public LimitOrderType Type { get; set; }
        public LimitOrderStatus Status { get; set; } = LimitOrderStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Cryptos Crypto { get; set; }
        public User User { get; set; }
    }
}
