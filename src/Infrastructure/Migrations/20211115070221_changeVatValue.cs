using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class changeVatValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Vats",
                newName: "Stavka");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stavka",
                table: "Vats",
                newName: "Value");
        }
    }
}
