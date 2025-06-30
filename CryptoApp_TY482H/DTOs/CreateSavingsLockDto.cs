namespace CryptoApp_TY482H.DTOs
{
    public class CreateSavingsLockDto
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public int DurationDays { get; set; }
    }
}
