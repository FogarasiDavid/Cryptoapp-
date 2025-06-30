using Microsoft.Extensions.Hosting;
using CryptoApp_TY482H.SqlServer;
using CryptoApp_TY482H.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp_TY482H.Services
{
    public class LimitOrderMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public LimitOrderMonitorService(IServiceScopeFactory fac) =>
            _scopeFactory = fac;

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var active = await db.LimitOrders
                    .Where(o => o.Status == LimitOrderStatus.Active
                             && o.ExpirationTime > DateTime.UtcNow)
                    .ToListAsync(ct);

                foreach (var o in active)
                {
                    var price = (await db.Cryptos.FindAsync(o.CryptoId)).CurrentPrice;
                    if ((o.Type == LimitOrderType.Buy && price <= o.LimitPrice)
                     || (o.Type == LimitOrderType.Sell && price >= o.LimitPrice))
                    {
                        o.Status = LimitOrderStatus.Executed;
                    }
                }

                await db.SaveChangesAsync(ct);
                await Task.Delay(_interval, ct);
            }
        }
    }
}
