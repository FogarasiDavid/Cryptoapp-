namespace CryptoApp_TY482H.DTOs
{
    public class CreateLimitOrderDto
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public decimal LimitPrice { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
