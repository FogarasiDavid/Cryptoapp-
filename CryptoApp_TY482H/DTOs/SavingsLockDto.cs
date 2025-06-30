using CryptoApp_TY482H.Entities;

namespace CryptoApp_TY482H.DTOs
{
    public class SavingsLockDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CryptoId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public decimal InterestRateAtLock { get; set; }
        public SavingsStatus Status { get; set; }
        public decimal MatureAmount { get; set; }
        public int UserId { get; set; }
    }
}
