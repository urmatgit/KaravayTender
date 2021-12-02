using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class ChangeDeliveryInPriceField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaComPosition_Areas_AreaId",
                table: "AreaComPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_AreaComPosition_ComPositions_ComPositionId",
                table: "AreaComPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AreaComPosition",
                table: "AreaComPosition");

            migrationBuilder.RenameTable(
                name: "AreaComPosition",
                newName: "AreaComPositions");

            migrationBuilder.RenameColumn(
                name: "IsPriceInDelivery",
                table: "ComOffers",
                newName: "IsDeliveryInPrice");

            migrationBuilder.RenameIndex(
                name: "IX_AreaComPosition_ComPositionId",
                table: "AreaComPositions",
                newName: "IX_AreaComPositions_ComPositionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AreaComPositions",
                table: "AreaComPositions",
                columns: new[] { "AreaId", "ComPositionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AreaComPositions_Areas_AreaId",
                table: "AreaComPositions",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AreaComPositions_ComPositions_ComPositionId",
                table: "AreaComPositions",
                column: "ComPositionId",
                principalTable: "ComPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreaComPositions_Areas_AreaId",
                table: "AreaComPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_AreaComPositions_ComPositions_ComPositionId",
                table: "AreaComPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AreaComPositions",
                table: "AreaComPositions");

            migrationBuilder.RenameTable(
                name: "AreaComPositions",
                newName: "AreaComPosition");

            migrationBuilder.RenameColumn(
                name: "IsDeliveryInPrice",
                table: "ComOffers",
                newName: "IsPriceInDelivery");

            migrationBuilder.RenameIndex(
                name: "IX_AreaComPositions_ComPositionId",
                table: "AreaComPosition",
                newName: "IX_AreaComPosition_ComPositionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AreaComPosition",
                table: "AreaComPosition",
                columns: new[] { "AreaId", "ComPositionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AreaComPosition_Areas_AreaId",
                table: "AreaComPosition",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AreaComPosition_ComPositions_ComPositionId",
                table: "AreaComPosition",
                column: "ComPositionId",
                principalTable: "ComPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
