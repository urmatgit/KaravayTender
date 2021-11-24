using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Razor.Infrastructure.Migrations
{
    public partial class AddCommersialOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaPosition");

            migrationBuilder.DropTable(
                name: "Lot");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.CreateTable(
                name: "ComOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<short>(type: "INTEGER", nullable: false),
                    Number = table.Column<string>(type: "TEXT", nullable: false),
                    DateBegin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DirectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TermBegin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TermEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ManagerId = table.Column<string>(type: "TEXT", nullable: false),
                    DelayDay = table.Column<short>(type: "INTEGER", nullable: false),
                    IsBankDays = table.Column<bool>(type: "INTEGER", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPriceInDelivery = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComOffers_Contragents_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComOffers_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComParticipants",
                columns: table => new
                {
                    ContragentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComOfferId = table.Column<int>(type: "INTEGER", nullable: false),
                    StepFailure = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<short>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComParticipants", x => new { x.ComOfferId, x.ContragentId });
                    table.ForeignKey(
                        name: "FK_ComParticipants_ComOffers_ComOfferId",
                        column: x => x.ComOfferId,
                        principalTable: "ComOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComParticipants_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeliveryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddRequirement = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Summa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SummaVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NomenclatureId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_ComPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComPositions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComPositions_ComOffers_ComOfferId",
                        column: x => x.ComOfferId,
                        principalTable: "ComOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComPositions_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Deadline = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_ComStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComStages_ComOffers_ComOfferId",
                        column: x => x.ComOfferId,
                        principalTable: "ComOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaComPosition",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComPositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaComPosition", x => new { x.AreaId, x.ComPositionId });
                    table.ForeignKey(
                        name: "FK_AreaComPosition_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaComPosition_ComPositions_ComPositionId",
                        column: x => x.ComPositionId,
                        principalTable: "ComPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StageCompositions",
                columns: table => new
                {
                    ComStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContragentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComPositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageCompositions", x => new { x.ComStageId, x.ContragentId, x.ComPositionId });
                    table.ForeignKey(
                        name: "FK_StageCompositions_ComPositions_ComPositionId",
                        column: x => x.ComPositionId,
                        principalTable: "ComPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageCompositions_ComStages_ComStageId",
                        column: x => x.ComStageId,
                        principalTable: "ComStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageCompositions_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaComPosition_ComPositionId",
                table: "AreaComPosition",
                column: "ComPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComOffers_DirectionId",
                table: "ComOffers",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComOffers_WinnerId",
                table: "ComOffers",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComParticipants_ContragentId",
                table: "ComParticipants",
                column: "ContragentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComPositions_CategoryId",
                table: "ComPositions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComPositions_ComOfferId",
                table: "ComPositions",
                column: "ComOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ComPositions_NomenclatureId",
                table: "ComPositions",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ComStages_ComOfferId",
                table: "ComStages",
                column: "ComOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_StageCompositions_ComPositionId",
                table: "StageCompositions",
                column: "ComPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_StageCompositions_ContragentId",
                table: "StageCompositions",
                column: "ContragentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaComPosition");

            migrationBuilder.DropTable(
                name: "ComParticipants");

            migrationBuilder.DropTable(
                name: "StageCompositions");

            migrationBuilder.DropTable(
                name: "ComPositions");

            migrationBuilder.DropTable(
                name: "ComStages");

            migrationBuilder.DropTable(
                name: "ComOffers");

            migrationBuilder.CreateTable(
                name: "Lot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DateBegin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DelayDay = table.Column<short>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBankDays = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsIncludeDelivery = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ManagerId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<short>(type: "INTEGER", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lot_Contragents_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lot_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddRequirement = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    NomenclatureId = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Position_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Position_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaPosition",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaPosition", x => new { x.AreaId, x.PositionId });
                    table.ForeignKey(
                        name: "FK_AreaPosition_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaPosition_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaPosition_PositionId",
                table: "AreaPosition",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lot_DirectionId",
                table: "Lot",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lot_WinnerId",
                table: "Lot",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Position_CategoryId",
                table: "Position",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Position_NomenclatureId",
                table: "Position",
                column: "NomenclatureId");
        }
    }
}
