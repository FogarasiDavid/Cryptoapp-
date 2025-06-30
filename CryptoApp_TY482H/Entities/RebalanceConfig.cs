namespace CryptoApp_TY482H.Entities
{
    public class RebalanceConfig
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AllocationJson { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }

    }
}
