namespace CryptoApp_TY482H.DTOs
{
    public class RebalancePreviewDto
    {
        public int UserId { get; set; }      
        public Guid ApprovalToken { get; set; }
        public List<TradeInstruction> Instructions { get; set; }
    }

    public class TradeInstruction
    {
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
}
