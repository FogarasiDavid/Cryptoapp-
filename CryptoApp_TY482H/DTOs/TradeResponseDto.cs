namespace CryptoApp_TY482H.DTOs
{
    public class TradeResponseDto
    {
        public int TransactionId { get; init; }
        public int UserId { get; init; }
        public int CryptoId { get; init; }
        public decimal Amount { get; init; }
        public decimal TotalAmount { get; init; }  
        public decimal Cashback { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
