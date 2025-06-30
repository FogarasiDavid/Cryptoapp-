using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoApp_TY482H.Migrations
{
    /// <inheritdoc />
    public partial class AddCryptoCascadeNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CryptoId",
                table: "Transactions",
                column: "CryptoId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitOrders_CryptoId",
                table: "LimitOrders",
                column: "CryptoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LimitOrders_Cryptos_CryptoId",
                table: "LimitOrders",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cryptos_CryptoId",
                table: "Transactions",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LimitOrders_Cryptos_CryptoId",
                table: "LimitOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cryptos_CryptoId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CryptoId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_LimitOrders_CryptoId",
                table: "LimitOrders");
        }
    }
}
