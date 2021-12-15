using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddContragentIdToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId1",
                table: "Contragents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContragentId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ManagerId1",
                table: "Contragents",
                column: "ManagerId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId1",
                table: "Contragents",
                column: "ManagerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Contragents_ContragentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId1",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId1",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ContragentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId1",
                table: "Contragents");

            migrationBuilder.DropColumn(
                name: "ContragentId",
                table: "AspNetUsers");

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
    }
}
