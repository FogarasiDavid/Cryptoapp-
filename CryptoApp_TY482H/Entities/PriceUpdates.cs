using BCrypt.Net;

namespace CryptoApp_TY482H.Entities
{
    public class PriceUpgrade
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime UpgradeDate { get; set; }

        public Cryptos Crypto { get; set; }
    }

}
