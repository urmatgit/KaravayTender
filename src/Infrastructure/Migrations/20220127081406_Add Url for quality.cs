using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddUrlforquality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "QualityDocs",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "QualityDocs");
        }
    }
}
