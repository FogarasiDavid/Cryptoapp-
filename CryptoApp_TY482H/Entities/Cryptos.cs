namespace CryptoApp_TY482H.Entities
{
    public class Cryptos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal InitialPrice { get; set; }
        public virtual ICollection<CryptoWallet> CryptoWallets { get; set; } = new HashSet<CryptoWallet>();
        public virtual ICollection<Transactions> Transactions { get; set; } = new HashSet<Transactions>();
        public virtual ICollection<LimitOrder> LimitOrders { get; set; } = new HashSet<LimitOrder>();
        public virtual ICollection<PriceUpgrade> PriceUpgrades { get; set; } = new HashSet<PriceUpgrade>();
    }

    
}
