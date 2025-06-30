namespace CryptoApp_TY482H.Entities
{
    public class RebalanceHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public string DetailsJson { get; set; }  
        public Guid ApprovalToken { get; set; }
        public User User { get; set; }
    }
}
