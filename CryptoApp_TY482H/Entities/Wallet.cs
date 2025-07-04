﻿namespace CryptoApp_TY482H.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public List<CryptoWallet> WalletCryptos { get; set; } = new List<CryptoWallet>();
    }
}
