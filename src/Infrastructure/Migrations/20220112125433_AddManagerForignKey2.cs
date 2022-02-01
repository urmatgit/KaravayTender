using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddManagerForignKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId",
                table: "Contragents",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents");

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
