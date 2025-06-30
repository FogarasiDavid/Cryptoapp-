namespace CryptoApp_TY482H.DTOs
{
    public class WalletDto
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public List<WalletCryptoDto> WalletCryptos { get; set; }
    }
}
