using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class MoveStatusToStageParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ComParticipants");

            migrationBuilder.DropColumn(
                name: "StepFailure",
                table: "ComParticipants");

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "StageParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StageParticipants");

            migrationBuilder.AddColumn<short>(
                name: "Status",
                table: "ComParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "StepFailure",
                table: "ComParticipants",
                type: "INTEGER",
                nullable: true);
        }
    }
}
