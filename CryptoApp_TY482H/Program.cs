using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.Services;
using CryptoApp_TY482H.Services.Interfaces;
using CryptoApp_TY482H.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "bearer",
        Type = SecuritySchemeType.Http,
        Description = "JWT Bearer auth"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("CryptoDbConnection"),
        sql => sql.EnableRetryOnFailure()
    )
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IPriceUpgradeService, PriceUpgradeService>();
builder.Services.AddScoped<IProfitService, ProfitService>();
builder.Services.AddScoped<ICashbackService, CashbackService>();
builder.Services.AddScoped<ISavingsService, SavingsService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(errApp =>
{
    errApp.Run(async ctx =>
    {
        ctx.Response.StatusCode = 500;
        ctx.Response.ContentType = "application/json";
        var feat = ctx.Features.Get<IExceptionHandlerFeature>();
        var msg = feat?.Error?.Message ?? "Ismeretlen hiba";
        await ctx.Response.WriteAsJsonAsync(new { error = msg });
    });
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Cryptos.Any())
    {
        db.Cryptos.AddRange(
            new Cryptos { Name = "Bitcoin", InitialPrice = 50000m, CurrentPrice = 50000m },
            new Cryptos { Name = "Ethereum", InitialPrice = 4000m, CurrentPrice = 4000m },
            new Cryptos { Name = "BinanceCoin", InitialPrice = 500m, CurrentPrice = 500m },
            new Cryptos { Name = "Cardano", InitialPrice = 2m, CurrentPrice = 2m },
            new Cryptos { Name = "Solana", InitialPrice = 150m, CurrentPrice = 150m },
            new Cryptos { Name = "XRP", InitialPrice = 1m, CurrentPrice = 1m },
            new Cryptos { Name = "Polkadot", InitialPrice = 25m, CurrentPrice = 25m },
            new Cryptos { Name = "Dogecoin", InitialPrice = 0.2m, CurrentPrice = 0.2m },
            new Cryptos { Name = "Avalanche", InitialPrice = 60m, CurrentPrice = 60m },
            new Cryptos { Name = "Litecoin", InitialPrice = 90m, CurrentPrice = 90m },
            new Cryptos { Name = "Chainlink", InitialPrice = 30m, CurrentPrice = 30m },
            new Cryptos { Name = "Polygon", InitialPrice = 1m, CurrentPrice = 1m },
            new Cryptos { Name = "Stellar", InitialPrice = 0.5m, CurrentPrice = 0.5m },
            new Cryptos { Name = "VeChain", InitialPrice = 0.1m, CurrentPrice = 0.1m },
            new Cryptos { Name = "TRON", InitialPrice = 0.07m, CurrentPrice = 0.07m }
        );
        db.SaveChanges();
    }

    if (!db.InterestRates.Any())
    {
        var allCryptoIds = db.Cryptos.Select(c => c.Id).ToList();
        foreach (var id in allCryptoIds)
        {
            db.InterestRates.Add(new InterestRate
            {
                CryptoId = id,
                Rate = 0.1m,
                EffectiveDate = DateTime.UtcNow
            });
        }
        db.SaveChanges();
    }

    if (!db.CashbackRules.Any())
    {
        db.CashbackRules.AddRange(
            new CashbackRule { MinAmount = 1000, MaxAmount = 3000, Percent = 1 },
            new CashbackRule { MinAmount = 3001, MaxAmount = 5000, Percent = 2 },
            new CashbackRule { MinAmount = 5001, MaxAmount = null, Percent = 3 }
        );
        db.SaveChanges();
    }

    if (!db.Users.Any())
    {
        var admin = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
            Role = "Admin"
        };
        db.Users.Add(admin);
        db.SaveChanges();

        db.Wallets.Add(new Wallet
        {
            UserId = admin.Id,
            Balance = 1000m
        });
        db.SaveChanges();
    }

    Console.WriteLine($"Total cryptos  : {db.Cryptos.Count()}");
    Console.WriteLine($"Total rates    : {db.InterestRates.Count()}");
    Console.WriteLine($"Total cashback : {db.CashbackRules.Count()}");
    Console.WriteLine($"Total users    : {db.Users.Count()}");
    Console.WriteLine($"Total wallets  : {db.Wallets.Count()}");
}

app.Run();
