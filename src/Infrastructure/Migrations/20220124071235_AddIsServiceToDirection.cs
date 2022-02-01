using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddIsServiceToDirection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsService",
                table: "Directions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsService",
                table: "Directions");
        }
    }
}
