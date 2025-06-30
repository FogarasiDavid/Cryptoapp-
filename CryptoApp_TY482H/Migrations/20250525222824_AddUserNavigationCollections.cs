using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoApp_TY482H.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNavigationCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RebalanceHistories_UserId",
                table: "RebalanceHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RebalanceConfigs_UserId",
                table: "RebalanceConfigs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitOrders_UserId",
                table: "LimitOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LimitOrders_Users_UserId",
                table: "LimitOrders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RebalanceConfigs_Users_UserId",
                table: "RebalanceConfigs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RebalanceHistories_Users_UserId",
                table: "RebalanceHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LimitOrders_Users_UserId",
                table: "LimitOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_RebalanceConfigs_Users_UserId",
                table: "RebalanceConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_RebalanceHistories_Users_UserId",
                table: "RebalanceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_RebalanceHistories_UserId",
                table: "RebalanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_RebalanceConfigs_UserId",
                table: "RebalanceConfigs");

            migrationBuilder.DropIndex(
                name: "IX_LimitOrders_UserId",
                table: "LimitOrders");
        }
    }
}
