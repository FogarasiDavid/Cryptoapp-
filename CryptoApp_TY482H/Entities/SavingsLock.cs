namespace CryptoApp_TY482H.Entities
{
    public enum SavingsStatus { Active, Completed, Cancelled }
    public class SavingsLock
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public decimal InterestRateAtLock { get; set; }
        public SavingsStatus Status { get; set; }

        public User User { get; set; }
        public Cryptos Crypto { get; set; }

        public decimal ComputeMatureAmount()
            => Amount * (1 + InterestRateAtLock / 100m * DurationDays);
        public decimal ComputeAccruedUntil(DateTime when)
        {
            var days = (when.Date - StartDate.Date).Days;
            if (days < 0) days = 0;
            if (days > DurationDays) days = DurationDays;
            return Amount * (1 + InterestRateAtLock / 100m * days);
        }
    }
}
