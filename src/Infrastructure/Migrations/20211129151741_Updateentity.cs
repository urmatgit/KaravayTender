using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class Updateentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Contragents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Contragents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "ComOffers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComOffers_ManagerId",
                table: "ComOffers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComOffers_AspNetUsers_ManagerId",
                table: "ComOffers",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ComOffers_AspNetUsers_ManagerId",
                table: "ComOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_ComOffers_ManagerId",
                table: "ComOffers");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Contragents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Contragents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "ComOffers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
