using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddComOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComOffers_Contragents_WinnerId",
                table: "ComOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComOffers_Directions_DirectionId",
                table: "ComOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComPositions_ComOffers_ComOfferId",
                table: "ComPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ComPositions_Nomenclatures_NomenclatureId",
                table: "ComPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ComParticipants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ComParticipants");

            migrationBuilder.AlterColumn<int>(
                name: "StepFailure",
                table: "ComParticipants",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "WinnerId",
                table: "ComOffers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "ComOffers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

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
                name: "FK_ComOffers_Contragents_WinnerId",
                table: "ComOffers",
                column: "WinnerId",
                principalTable: "Contragents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComOffers_Directions_DirectionId",
                table: "ComOffers",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComPositions_ComOffers_ComOfferId",
                table: "ComPositions",
                column: "ComOfferId",
                principalTable: "ComOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComPositions_Nomenclatures_NomenclatureId",
                table: "ComPositions",
                column: "NomenclatureId",
                principalTable: "Nomenclatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ComOffers_Contragents_WinnerId",
                table: "ComOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComOffers_Directions_DirectionId",
                table: "ComOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComPositions_ComOffers_ComOfferId",
                table: "ComPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ComPositions_Nomenclatures_NomenclatureId",
                table: "ComPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contragents_AspNetUsers_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_Contragents_ManagerId",
                table: "Contragents");

            migrationBuilder.DropIndex(
                name: "IX_ComOffers_ManagerId",
                table: "ComOffers");

            migrationBuilder.AlterColumn<int>(
                name: "StepFailure",
                table: "ComParticipants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "ComParticipants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ComParticipants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WinnerId",
                table: "ComOffers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "ComOffers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_ApplicationUserId",
                table: "Contragents",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComOffers_Contragents_WinnerId",
                table: "ComOffers",
                column: "WinnerId",
                principalTable: "Contragents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComOffers_Directions_DirectionId",
                table: "ComOffers",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComPositions_ComOffers_ComOfferId",
                table: "ComPositions",
                column: "ComOfferId",
                principalTable: "ComOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComPositions_Nomenclatures_NomenclatureId",
                table: "ComPositions",
                column: "NomenclatureId",
                principalTable: "Nomenclatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
