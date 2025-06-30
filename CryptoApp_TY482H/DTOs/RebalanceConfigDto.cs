namespace CryptoApp_TY482H.DTOs
{
    public class RebalanceConfigDto
    {
        public int UserId { get; set; }
        public List<AllocationItem> Allocations { get; set; }
    }

    public class AllocationItem
    {
        public int CryptoId { get; set; }
        public decimal TargetPercent { get; set; }
    }
}
