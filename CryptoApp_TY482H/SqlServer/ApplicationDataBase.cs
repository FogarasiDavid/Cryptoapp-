using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.Entities;

namespace CryptoApp_TY482H.SqlServer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CryptoWallet> CryptoWallets { get; set; }
        public DbSet<Cryptos> Cryptos { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<LimitOrder> LimitOrders { get; set; }
        public DbSet<RebalanceConfig> RebalanceConfigs { get; set; }
        public DbSet<RebalanceHistory> RebalanceHistories { get; set; }
        public DbSet<PriceUpgrade> PriceUpgrades { get; set; }
        public DbSet<CashbackRule> CashbackRules { get; set; }
        public DbSet<InterestRate> InterestRates { get; set; }
        public DbSet<SavingsLock> SavingsLocks  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transactions>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Transactions>()
                .Property(t => t.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PriceUpgrade>()
                .Property(pu => pu.NewPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cryptos>(c =>
            {
                c.Property(x => x.CurrentPrice).HasPrecision(18, 2);
                c.Property(x => x.InitialPrice).HasPrecision(18, 2);

                c.HasMany(x => x.CryptoWallets)
                 .WithOne(cw => cw.Crypto)
                 .HasForeignKey(cw => cw.CryptoId)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.Transactions)
                 .WithOne(t => t.Crypto)
                 .HasForeignKey(t => t.CryptoId)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.LimitOrders)
                 .WithOne(lo => lo.Crypto)
                 .HasForeignKey(lo => lo.CryptoId)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.PriceUpgrades)
                 .WithOne(pu => pu.Crypto)
                 .HasForeignKey(pu => pu.CryptoId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CryptoWallet>()
                .Property(cw => cw.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LimitOrder>(lo =>
            {
                lo.Property(x => x.Amount).HasPrecision(18, 2);
                lo.Property(x => x.LimitPrice).HasPrecision(18, 2);
            });
            modelBuilder.Entity<CashbackRule>(entity =>
            {
                entity.Property(c => c.MinAmount)
                      .HasPrecision(18, 2);
                entity.Property(c => c.MaxAmount)
                      .HasPrecision(18, 2);
                entity.Property(c => c.Percent)
                      .HasPrecision(5, 2);
            });
            modelBuilder.Entity<InterestRate>(entity =>
            {
                entity.Property(i => i.Rate)
                      .HasPrecision(5, 4);
            });
            modelBuilder.Entity<SavingsLock>(entity =>
            {
                entity.Property(s => s.Amount)
                      .HasPrecision(18, 2);
                entity.Property(s => s.InterestRateAtLock)
                      .HasPrecision(5, 4);
            });
            modelBuilder.Entity<User>(u =>
            {
                u.HasOne(x => x.Wallet)
                 .WithOne(w => w.User)
                 .HasForeignKey<Wallet>(w => w.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                u.HasMany(x => x.Transactions)
                 .WithOne(t => t.User)
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                u.HasMany(x => x.LimitOrders)
                 .WithOne(lo => lo.User)
                 .HasForeignKey(lo => lo.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                u.HasMany(x => x.RebalanceConfigs)
                 .WithOne(rc => rc.User)
                 .HasForeignKey(rc => rc.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                u.HasMany(x => x.RebalanceHistories)
                 .WithOne(rh => rh.User)
                 .HasForeignKey(rh => rh.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
