using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddManagerForignKey4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Contragents_ContragentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ContragentId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ContragentId",
                table: "AspNetUsers",
                column: "ContragentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Contragents_ContragentId",
                table: "AspNetUsers",
                column: "ContragentId",
                principalTable: "Contragents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
