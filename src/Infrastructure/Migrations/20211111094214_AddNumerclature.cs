using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddNumerclature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclature_Directions_DirectionId",
                table: "Nomenclature");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclature_UnitOfs_UnitId",
                table: "Nomenclature");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclature_Vats_VatId",
                table: "Nomenclature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nomenclature",
                table: "Nomenclature");

            migrationBuilder.RenameTable(
                name: "Nomenclature",
                newName: "Nomenclatures");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Nomenclatures",
                newName: "UnitOfId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclature_VatId",
                table: "Nomenclatures",
                newName: "IX_Nomenclatures_VatId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclature_UnitId",
                table: "Nomenclatures",
                newName: "IX_Nomenclatures_UnitOfId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclature_DirectionId",
                table: "Nomenclatures",
                newName: "IX_Nomenclatures_DirectionId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Nomenclatures",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Specification",
                table: "Nomenclatures",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "Nomenclatures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nomenclatures",
                table: "Nomenclatures",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QualityDocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NomenclatureQualityDocs",
                columns: table => new
                {
                    NomenclatureId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualityDocId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NomenclatureQualityDocs", x => new { x.NomenclatureId, x.QualityDocId });
                    table.ForeignKey(
                        name: "FK_NomenclatureQualityDocs_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NomenclatureQualityDocs_QualityDocs_QualityDocId",
                        column: x => x.QualityDocId,
                        principalTable: "QualityDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nomenclatures_CategoryId",
                table: "Nomenclatures",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NomenclatureQualityDocs_QualityDocId",
                table: "NomenclatureQualityDocs",
                column: "QualityDocId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclatures_Categories_CategoryId",
                table: "Nomenclatures",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclatures_Directions_DirectionId",
                table: "Nomenclatures",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclatures_UnitOfs_UnitOfId",
                table: "Nomenclatures",
                column: "UnitOfId",
                principalTable: "UnitOfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclatures_Vats_VatId",
                table: "Nomenclatures",
                column: "VatId",
                principalTable: "Vats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclatures_Categories_CategoryId",
                table: "Nomenclatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclatures_Directions_DirectionId",
                table: "Nomenclatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclatures_UnitOfs_UnitOfId",
                table: "Nomenclatures");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomenclatures_Vats_VatId",
                table: "Nomenclatures");

            migrationBuilder.DropTable(
                name: "NomenclatureQualityDocs");

            migrationBuilder.DropTable(
                name: "QualityDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nomenclatures",
                table: "Nomenclatures");

            migrationBuilder.DropIndex(
                name: "IX_Nomenclatures_CategoryId",
                table: "Nomenclatures");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Nomenclatures");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "Nomenclatures");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Nomenclatures");

            migrationBuilder.RenameTable(
                name: "Nomenclatures",
                newName: "Nomenclature");

            migrationBuilder.RenameColumn(
                name: "UnitOfId",
                table: "Nomenclature",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclatures_VatId",
                table: "Nomenclature",
                newName: "IX_Nomenclature_VatId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclatures_UnitOfId",
                table: "Nomenclature",
                newName: "IX_Nomenclature_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomenclatures_DirectionId",
                table: "Nomenclature",
                newName: "IX_Nomenclature_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nomenclature",
                table: "Nomenclature",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclature_Directions_DirectionId",
                table: "Nomenclature",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclature_UnitOfs_UnitId",
                table: "Nomenclature",
                column: "UnitId",
                principalTable: "UnitOfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomenclature_Vats_VatId",
                table: "Nomenclature",
                column: "VatId",
                principalTable: "Vats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
