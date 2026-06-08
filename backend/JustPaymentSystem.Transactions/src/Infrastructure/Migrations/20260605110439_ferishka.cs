using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ferishka : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSnapshots_Transactions_TransactionId",
                table: "PaymentSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_PaymentSnapshots_TransactionId",
                table: "PaymentSnapshots");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentSnapshotId",
                table: "Transactions",
                column: "PaymentSnapshotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentSnapshots_PaymentSnapshotId",
                table: "Transactions",
                column: "PaymentSnapshotId",
                principalTable: "PaymentSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentSnapshots_PaymentSnapshotId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentSnapshotId",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSnapshots_TransactionId",
                table: "PaymentSnapshots",
                column: "TransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSnapshots_Transactions_TransactionId",
                table: "PaymentSnapshots",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
