using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddedStageParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StageParticipants",
                columns: table => new
                {
                    ComStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContragentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComOfferId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageParticipants", x => new { x.ComStageId, x.ContragentId, x.ComOfferId });
                    table.ForeignKey(
                        name: "FK_StageParticipants_ComOffers_ComOfferId",
                        column: x => x.ComOfferId,
                        principalTable: "ComOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageParticipants_ComStages_ComStageId",
                        column: x => x.ComStageId,
                        principalTable: "ComStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageParticipants_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageParticipants_ComOfferId",
                table: "StageParticipants",
                column: "ComOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_StageParticipants_ContragentId",
                table: "StageParticipants",
                column: "ContragentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StageParticipants");
        }
    }
}
