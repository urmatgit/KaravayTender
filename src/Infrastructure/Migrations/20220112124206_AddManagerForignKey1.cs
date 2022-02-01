using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddManagerForignKey1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId1",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId1",
                table: "Contragents");

            migrationBuilder.DropColumn(
                name: "ManagerId1",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId1",
                table: "Contragents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ManagerId1",
                table: "Contragents",
                column: "ManagerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId1",
                table: "Contragents",
                column: "ManagerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
