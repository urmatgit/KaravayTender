using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddArchiveField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specification",
                table: "Nomenclatures",
                newName: "Specifications");

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "Nomenclatures",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archive",
                table: "Nomenclatures");

            migrationBuilder.RenameColumn(
                name: "Specifications",
                table: "Nomenclatures",
                newName: "Specification");
        }
    }
}
