﻿// <auto-generated />
using System;
using CryptoApp_TY482H.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptoApp_TY482H.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250525223213_AddCascadeToUserRelations")]
    partial class AddCascadeToUserRelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CryptoApp_TY482H.Entities.CryptoWallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CryptoId")
                        .HasColumnType("int");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptoId");

                    b.HasIndex("WalletId");

                    b.ToTable("CryptoWallets");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Cryptos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CurrentPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cryptos");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.LimitOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CryptoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LimitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptoId");

                    b.HasIndex("UserId");

                    b.ToTable("LimitOrders");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.PriceUpgrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CryptoId")
                        .HasColumnType("int");

                    b.Property<decimal>("NewPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpgradeDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CryptoId");

                    b.ToTable("PriceUpgrades");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.RebalanceConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AllocationJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RebalanceConfigs");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.RebalanceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ApprovalToken")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DetailsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExecutedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RebalanceHistories");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Transactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CryptoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptoId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.CryptoWallet", b =>
                {
                    b.HasOne("CryptoApp_TY482H.Entities.Cryptos", "Crypto")
                        .WithMany("CryptoWallets")
                        .HasForeignKey("CryptoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoApp_TY482H.Entities.Wallet", "Wallet")
                        .WithMany("WalletCryptos")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crypto");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.LimitOrder", b =>
                {
                    b.HasOne("CryptoApp_TY482H.Entities.Cryptos", "Crypto")
                        .WithMany("LimitOrders")
                        .HasForeignKey("CryptoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("LimitOrders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crypto");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.PriceUpgrade", b =>
                {
                    b.HasOne("CryptoApp_TY482H.Entities.Cryptos", "Crypto")
                        .WithMany("PriceUpgrades")
                        .HasForeignKey("CryptoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crypto");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.RebalanceConfig", b =>
                {
                    b.HasOne("User", "User")
                        .WithMany("RebalanceConfigs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.RebalanceHistory", b =>
                {
                    b.HasOne("User", "User")
                        .WithMany("RebalanceHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Transactions", b =>
                {
                    b.HasOne("CryptoApp_TY482H.Entities.Cryptos", "Crypto")
                        .WithMany("Transactions")
                        .HasForeignKey("CryptoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crypto");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Wallet", b =>
                {
                    b.HasOne("User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("CryptoApp_TY482H.Entities.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Cryptos", b =>
                {
                    b.Navigation("CryptoWallets");

                    b.Navigation("LimitOrders");

                    b.Navigation("PriceUpgrades");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CryptoApp_TY482H.Entities.Wallet", b =>
                {
                    b.Navigation("WalletCryptos");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("LimitOrders");

                    b.Navigation("RebalanceConfigs");

                    b.Navigation("RebalanceHistories");

                    b.Navigation("Transactions");

                    b.Navigation("Wallet")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
