using Microsoft.EntityFrameworkCore.Migrations;

namespace SwissBank.Data.Migrations
{
    public partial class UpdadeRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses");

            migrationBuilder.AlterColumn<string>(
                name: "ReseaverId",
                table: "Transactionses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses",
                column: "ReseaverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses");

            migrationBuilder.AlterColumn<string>(
                name: "ReseaverId",
                table: "Transactionses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Transactionses_AspNetUsers_ReseaverId",
                table: "Transactionses",
                column: "ReseaverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
