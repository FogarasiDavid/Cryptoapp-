namespace CryptoApp_TY482H.Entities
{
    public class CryptoWallet
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
        public Wallet Wallet { get; set; }
        public Cryptos Crypto { get; set; }

    }
}
