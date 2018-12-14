using Microsoft.EntityFrameworkCore.Migrations;

namespace SwissBank.Data.Migrations
{
    public partial class logg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses");

            migrationBuilder.RenameColumn(
                name: "ReseaverId",
                table: "Transactionses",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactionses_ReseaverId",
                table: "Transactionses",
                newName: "IX_Transactionses_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReceiverId",
                table: "Transactionses",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReceiverId",
                table: "Transactionses");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transactionses",
                newName: "ReseaverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactionses_ReceiverId",
                table: "Transactionses",
                newName: "IX_Transactionses_ReseaverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses",
                column: "ReseaverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
