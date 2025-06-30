namespace CryptoApp_TY482H.Entities
{
    public class InterestRate
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public Cryptos Crypto { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

}
