using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class ContragentCategoryCascadDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContragentCategories_Categories_CategoryId",
                table: "ContragentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContragentCategories_Contragents_ContragentId",
                table: "ContragentCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ContragentCategories_Categories_CategoryId",
                table: "ContragentCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContragentCategories_Contragents_ContragentId",
                table: "ContragentCategories",
                column: "ContragentId",
                principalTable: "Contragents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContragentCategories_Categories_CategoryId",
                table: "ContragentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContragentCategories_Contragents_ContragentId",
                table: "ContragentCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ContragentCategories_Categories_CategoryId",
                table: "ContragentCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContragentCategories_Contragents_ContragentId",
                table: "ContragentCategories",
                column: "ContragentId",
                principalTable: "Contragents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
