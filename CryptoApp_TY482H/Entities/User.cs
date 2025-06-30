using CryptoApp_TY482H.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } = "User";
    public Wallet Wallet { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Transactions> Transactions { get; set; }
        = new HashSet<Transactions>();

    public ICollection<LimitOrder> LimitOrders { get; set; }
        = new HashSet<LimitOrder>();

    public ICollection<RebalanceConfig> RebalanceConfigs { get; set; }
        = new HashSet<RebalanceConfig>();

    public ICollection<RebalanceHistory> RebalanceHistories { get; set; }
        = new HashSet<RebalanceHistory>();
}